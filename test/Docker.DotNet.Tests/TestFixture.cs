using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Docker.DotNet.X509;

namespace Docker.DotNet.Tests;

[CollectionDefinition(nameof(TestCollection))]
public sealed class TestCollection : ICollectionFixture<TestFixture>;

public sealed class TestFixture : Progress<JSONMessage>, IAsyncLifetime, IDisposable, ILogger
{
    private const LogLevel MinLogLevel = LogLevel.Debug;

    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private readonly IMessageSink _messageSink;

    private Dictionary<TestDaemonsEnum, bool> _isInitialized = new();
    private Dictionary<TestDaemonsEnum, bool> _isDisposed = new();
    private Dictionary<TestDaemonsEnum, bool> _hasInitializedSwarm = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestFixture" /> class.
    /// </summary>
    /// <param name="messageSink">The message sink.</param>
    /// <exception cref="TimeoutException">Thrown when tests are not completed within 5 minutes.</exception>
    public TestFixture(IMessageSink messageSink)
    {
        _messageSink = messageSink;

        DockerClients = new Dictionary<TestClientsEnum, DockerClient>
        {
            { TestClientsEnum.ManagedPipe, new DockerClientConfiguration().CreateClient(logger: this) },
            { TestClientsEnum.ManagedHttp, new DockerClientConfiguration(endpoint: new Uri("http://localhost:2375")).CreateClient(logger: this) },
            { TestClientsEnum.NativeHttp, new DockerClientConfiguration(endpoint: new Uri("http://localhost:2375"), nativeHttpHandler: true).CreateClient(logger: this) },
        };

        try
        {
            var tempDir = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");
#if NET9_0_OR_GREATER
            var credentials = new CertificateCredentials(X509CertificateLoader.LoadPkcs12FromFile(Path.Combine(tempDir, "certs", "client.pfx"), ""))
            {
                ServerCertificateValidationCallback = ValidateServerCertificate
            };
#else
            var credentials = new CertificateCredentials(new X509Certificate2(Path.Combine(tempDir, "certs", "client.pfx"), ""))
            {
                ServerCertificateValidationCallback = ValidateServerCertificate
            };
#endif
            DockerClients.Add(TestClientsEnum.ManagedHttps, new DockerClientConfiguration(endpoint: new Uri("http://localhost:2376"), credentials).CreateClient(logger: this));
            DockerClients.Add(TestClientsEnum.NativeHttps, new DockerClientConfiguration(endpoint: new Uri("http://localhost:2376"), credentials, nativeHttpHandler: true).CreateClient(logger: this));
        }
        catch (Exception ex)
        {
            this.LogWarning(ex, "Couldn't init tls clients because of certificate errors.");
        }


        Images = new Dictionary<TestDaemonsEnum, ImagesListResponse>();
        Cts = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        Cts.Token.Register(() => throw new TimeoutException("Docker.DotNet tests timed out."));
    }

    internal static bool ValidateServerCertificate(
            object sender,
            X509Certificate cert,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    /// <summary>
    /// Gets the Docker image repository.
    /// </summary>
    public string Repository { get; }
        = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the Docker image tag.
    /// </summary>
    public string Tag { get; }
        = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the Docker clients.
    /// </summary>
    public Dictionary<TestClientsEnum, DockerClient> DockerClients { get; }

    /// <summary>
    /// Gets the cancellation token source.
    /// </summary>
    public CancellationTokenSource Cts { get; }

    /// <summary>
    /// Gets or sets the Docker image.
    /// </summary>
    public Dictionary<TestDaemonsEnum, ImagesListResponse> Images { get; private set; }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        const string repository = "alpine";

        const string tag = "3.20";

        foreach (TestDaemonsEnum daemon in Enum.GetValues(typeof(TestDaemonsEnum)))
        {
            if (_isInitialized.TryGetValue(daemon, out var value) && value)
                continue;

            // Create image
            await DockerClients[GetClientForDaemon(daemon)].Images.CreateImageAsync(new ImagesCreateParameters { FromImage = repository, Tag = tag }, null, this, Cts.Token)
                .ConfigureAwait(false);

            // Get images
            var images = await DockerClients[GetClientForDaemon(daemon)].Images.ListImagesAsync(
                    new ImagesListParameters
                    {
                        Filters = new Dictionary<string, IDictionary<string, bool>>
                        {
                            ["reference"] = new Dictionary<string, bool>
                            {
                                [repository + ":" + tag] = true
                            }
                        }
                    }, Cts.Token)
                .ConfigureAwait(false);

            // Set image
            Images.Add(daemon, images.Single());

            // Tag image
            await DockerClients[GetClientForDaemon(daemon)].Images.TagImageAsync(Images[daemon].ID, new ImageTagParameters { RepositoryName = Repository, Tag = Tag }, Cts.Token)
                .ConfigureAwait(false);

            // Init a new swarm, if not part of an existing one
            try
            {
                _ = await DockerClients[GetClientForDaemon(daemon)].Swarm.InitSwarmAsync(new SwarmInitParameters { AdvertiseAddr = "10.10.10.10", ListenAddr = "127.0.0.1" }, Cts.Token)
                    .ConfigureAwait(false);

                _hasInitializedSwarm.Add(daemon, true);
            }
            catch
            {
                this.LogInformation("Couldn't init a new swarm, the node should take part of an existing one.");

                _hasInitializedSwarm.Add(daemon, false);
            }

            _isInitialized.Add(daemon, false);
        }
    }

    public static TestDaemonsEnum GetDaemonForClient(TestClientsEnum client)
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            return client switch
            {
                TestClientsEnum.ManagedPipe => TestDaemonsEnum.Local,
                TestClientsEnum.ManagedHttp => TestDaemonsEnum.DindHttp,
                TestClientsEnum.NativeHttp => TestDaemonsEnum.DindHttp,
                TestClientsEnum.ManagedHttps => TestDaemonsEnum.DindHttps,
                TestClientsEnum.NativeHttps => TestDaemonsEnum.DindHttps,
                _ => throw new ArgumentOutOfRangeException(nameof(client), client, null)
            };
        }
        else
        {
            return client switch
            {
                TestClientsEnum.ManagedPipe => TestDaemonsEnum.Local,
                TestClientsEnum.ManagedHttp => TestDaemonsEnum.Local,
                TestClientsEnum.NativeHttp => TestDaemonsEnum.Local,
                _ => throw new ArgumentOutOfRangeException(nameof(client), client, null)
            };
        }
    }

    public static TestClientsEnum GetClientForDaemon(TestDaemonsEnum daemon)
    {
        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
        {
            return daemon switch
            {
                TestDaemonsEnum.Local => TestClientsEnum.ManagedPipe,
                TestDaemonsEnum.DindHttp => TestClientsEnum.ManagedHttp,
                TestDaemonsEnum.DindHttps => TestClientsEnum.ManagedHttps,
                _ => throw new ArgumentOutOfRangeException(nameof(daemon), daemon, null)

            };
        }
        else
        {
            return daemon switch
            {
                TestDaemonsEnum.Local => TestClientsEnum.ManagedPipe,
                TestDaemonsEnum.DindHttp => TestClientsEnum.ManagedPipe,
                TestDaemonsEnum.DindHttps => TestClientsEnum.ManagedPipe,
                _ => throw new ArgumentOutOfRangeException(nameof(daemon), daemon, null)
            };
        }
    }

    public static IEnumerable<object[]> GetDockerClientTypes()
    {
        var allClients = Enum.GetValues(typeof(TestClientsEnum))
                            .Cast<TestClientsEnum>();

        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != "true")
        {
            return allClients
                .Where(t => t == TestClientsEnum.ManagedPipe ||
                            t == TestClientsEnum.ManagedHttp ||
                            t == TestClientsEnum.NativeHttp)
                .Select(t => new object[] { t });
        }

        return allClients.Select(t => new object[] { t });
    }

    public static IEnumerable<TestDaemonsEnum> GetDockerDaemonTypes()
    {
        var allDaemons = Enum.GetValues(typeof(TestDaemonsEnum))
                            .Cast<TestDaemonsEnum>();

        if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") != "true")
        {
            return allDaemons
                .Where(t => t == TestDaemonsEnum.Local);
        }

        return allDaemons;
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        foreach (TestDaemonsEnum daemon in GetDockerDaemonTypes())
        {
            if (_isDisposed.TryGetValue(daemon, out var disposed) && disposed)
                continue;

            if (_hasInitializedSwarm.TryGetValue(daemon, out var swarm) && swarm)
            {
                await DockerClients[GetClientForDaemon(daemon)].Swarm.LeaveSwarmAsync(new SwarmLeaveParameters { Force = true }, Cts.Token)
                    .ConfigureAwait(false);
            }

            var containers = await DockerClients[GetClientForDaemon(daemon)].Containers.ListContainersAsync(
                    new ContainersListParameters
                    {
                        Filters = new Dictionary<string, IDictionary<string, bool>>
                        {
                            ["ancestor"] = new Dictionary<string, bool>
                            {
                                [Images[daemon].ID] = true
                            }
                        },
                        All = true
                    }, Cts.Token)
                .ConfigureAwait(false);

            var images = await DockerClients[GetClientForDaemon(daemon)].Images.ListImagesAsync(
                    new ImagesListParameters
                    {
                        Filters = new Dictionary<string, IDictionary<string, bool>>
                        {
                            ["reference"] = new Dictionary<string, bool>
                            {
                                [Images[daemon].ID] = true
                            }
                        },
                        All = true
                    }, Cts.Token)
                .ConfigureAwait(false);

            foreach (var container in containers)
            {
                await DockerClients[GetClientForDaemon(daemon)].Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters { Force = true }, Cts.Token)
                    .ConfigureAwait(false);
            }

            foreach (var image in images)
            {
                await DockerClients[GetClientForDaemon(daemon)].Images.DeleteImageAsync(image.ID, new ImageDeleteParameters { Force = true }, Cts.Token)
                    .ConfigureAwait(false);
            }

            _isDisposed.Add(daemon, true);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Cts.Dispose();
        foreach (var client in DockerClients.Values)
        {
            client?.Dispose();
        }
        DockerClients.Clear();
    }

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (IsEnabled(logLevel))
        {
            var message = exception == null ? formatter.Invoke(state, null) : string.Join(Environment.NewLine, formatter.Invoke(state, exception), exception);
            _messageSink.OnMessage(new DiagnosticMessage(string.Format("[Docker.DotNet {0:hh\\:mm\\:ss\\.ff}] {1}", _stopwatch.Elapsed, message)));
        }
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == MinLogLevel;
    }

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return new Disposable();
    }

    /// <inheritdoc />
    protected override void OnReport(JSONMessage value)
    {
        var message = JsonSerializer.Instance.Serialize(value);
        this.LogInformation("Progress: '{Progress}'.", message);
    }

    private sealed class Disposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
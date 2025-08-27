namespace Docker.DotNet.Tests;

[CollectionDefinition(nameof(TestCollection))]
public sealed class TestCollection : ICollectionFixture<TestFixture>;

public sealed class TestFixture : Progress<JSONMessage>, IAsyncLifetime, IDisposable, ILogger
{
    private const LogLevel MinLogLevel = LogLevel.Debug;

    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    private readonly IMessageSink _messageSink;

    private bool _hasInitializedSwarm;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestFixture" /> class.
    /// </summary>
    /// <param name="messageSink">The message sink.</param>
    /// <exception cref="TimeoutException">Thrown when tests are not completed within 5 minutes.</exception>
    public TestFixture(IMessageSink messageSink)
    {
        _messageSink = messageSink;
        DockerClients = new Dictionary<DockerClientType, DockerClient>
        {
            { DockerClientType.ManagedPipe, new DockerClientConfiguration().CreateClient(logger: this) },
            { DockerClientType.ManagedHttp, new DockerClientConfiguration(endpoint: new Uri("http://localhost:2375")).CreateClient(logger: this) },
            { DockerClientType.NativeHttp, new DockerClientConfiguration(endpoint: new Uri("http://localhost:2375"), nativeHttpHandler: true).CreateClient(logger: this) }
        };
        Cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
        Cts.Token.Register(() => throw new TimeoutException("Docker.DotNet tests timed out."));
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
    public Dictionary<DockerClientType, DockerClient> DockerClients { get; }

    /// <summary>
    /// Gets the cancellation token source.
    /// </summary>
    public CancellationTokenSource Cts { get; }

    /// <summary>
    /// Gets or sets the Docker image.
    /// </summary>
    public ImagesListResponse Image { get; private set; }

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        const string repository = "alpine";

        const string tag = "3.20";

        // Create image
        await DockerClients[DockerClientType.ManagedPipe].Images.CreateImageAsync(new ImagesCreateParameters { FromImage = repository, Tag = tag }, null, this, Cts.Token)
            .ConfigureAwait(false);

        // Get images
        var images = await DockerClients[DockerClientType.ManagedPipe].Images.ListImagesAsync(
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
        Image = images.Single();

        // Tag image
        await DockerClients[DockerClientType.ManagedPipe].Images.TagImageAsync(Image.ID, new ImageTagParameters { RepositoryName = Repository, Tag = Tag }, Cts.Token)
            .ConfigureAwait(false);

        // Init a new swarm, if not part of an existing one
        try
        {
            _ = await DockerClients[DockerClientType.ManagedPipe].Swarm.InitSwarmAsync(new SwarmInitParameters { AdvertiseAddr = "10.10.10.10", ListenAddr = "127.0.0.1" }, Cts.Token)
                .ConfigureAwait(false);

            _hasInitializedSwarm = true;
        }
        catch
        {
            this.LogInformation("Couldn't init a new swarm, the node should take part of an existing one.");

            _hasInitializedSwarm = false;
        }
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        if (_hasInitializedSwarm)
        {
            await DockerClients[DockerClientType.ManagedPipe].Swarm.LeaveSwarmAsync(new SwarmLeaveParameters { Force = true }, Cts.Token)
                .ConfigureAwait(false);
        }

        var containers = await DockerClients[DockerClientType.ManagedPipe].Containers.ListContainersAsync(
                new ContainersListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        ["ancestor"] = new Dictionary<string, bool>
                        {
                            [Image.ID] = true
                        }
                    },
                    All = true
                }, Cts.Token)
            .ConfigureAwait(false);

        var images = await DockerClients[DockerClientType.ManagedPipe].Images.ListImagesAsync(
                new ImagesListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        ["reference"] = new Dictionary<string, bool>
                        {
                            [Image.ID] = true
                        }
                    },
                    All = true
                }, Cts.Token)
            .ConfigureAwait(false);

        foreach (var container in containers)
        {
            await DockerClients[DockerClientType.ManagedPipe].Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters { Force = true }, Cts.Token)
                .ConfigureAwait(false);
        }

        foreach (var image in images)
        {
            await DockerClients[DockerClientType.ManagedPipe].Images.DeleteImageAsync(image.ID, new ImageDeleteParameters { Force = true }, Cts.Token)
                .ConfigureAwait(false);
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
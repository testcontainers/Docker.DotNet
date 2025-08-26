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
        DockerClientConfiguration = new DockerClientConfiguration(nativeHttpHandler: true);
        DockerClient = DockerClientConfiguration.CreateClient(logger: this);
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
    /// Gets the Docker client configuration.
    /// </summary>
    public DockerClientConfiguration DockerClientConfiguration { get; }

    /// <summary>
    /// Gets the Docker client.
    /// </summary>
    public DockerClient DockerClient { get; }

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
        await DockerClient.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = repository, Tag = tag }, null, this, Cts.Token)
            .ConfigureAwait(false);

        // Get images
        var images = await DockerClient.Images.ListImagesAsync(
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
        await DockerClient.Images.TagImageAsync(Image.ID, new ImageTagParameters { RepositoryName = Repository, Tag = Tag }, Cts.Token)
            .ConfigureAwait(false);

        // Init a new swarm, if not part of an existing one
        try
        {
            _ = await DockerClient.Swarm.InitSwarmAsync(new SwarmInitParameters { AdvertiseAddr = "10.10.10.10", ListenAddr = "127.0.0.1" }, Cts.Token)
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
            await DockerClient.Swarm.LeaveSwarmAsync(new SwarmLeaveParameters { Force = true }, Cts.Token)
                .ConfigureAwait(false);
        }

        var containers = await DockerClient.Containers.ListContainersAsync(
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

        var images = await DockerClient.Images.ListImagesAsync(
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
            await DockerClient.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters { Force = true }, Cts.Token)
                .ConfigureAwait(false);
        }

        foreach (var image in images)
        {
            await DockerClient.Images.DeleteImageAsync(image.ID, new ImageDeleteParameters { Force = true }, Cts.Token)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Cts.Dispose();
        DockerClient.Dispose();
        DockerClientConfiguration.Dispose();
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
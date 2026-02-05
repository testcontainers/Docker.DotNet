namespace Docker.DotNet;

using System;

public class DockerClientConfiguration : IDockerClientConfiguration, IDisposable
{
    private static readonly bool NativeHttpEnabled = Environment.GetEnvironmentVariable("DOCKER_DOTNET_NATIVE_HTTP_ENABLED") == "1";

    public DockerClientConfiguration(
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null,
        SocketConnectionConfiguration socketConfiguration = null,
        Action<HttpMessageHandler> configureHandler = null)
        : this(GetLocalDockerEndpoint(), credentials, defaultTimeout, namedPipeConnectTimeout, defaultHttpRequestHeaders, socketConfiguration, configureHandler)
    {
    }

    public DockerClientConfiguration(
        Uri endpoint,
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null,
        SocketConnectionConfiguration socketConfiguration = null,
        Action<HttpMessageHandler> configureHandler = null)
    {
        if (endpoint == null)
        {
            throw new ArgumentNullException(nameof(endpoint));
        }

        if (defaultTimeout < Timeout.InfiniteTimeSpan)
        {
            throw new ArgumentException("Default timeout must be greater than -1", nameof(defaultTimeout));
        }

        EndpointBaseUri = endpoint;
        Credentials = credentials ?? new AnonymousCredentials();
        DefaultTimeout = TimeSpan.Equals(TimeSpan.Zero, defaultTimeout) ? TimeSpan.FromSeconds(100) : defaultTimeout;
        NamedPipeConnectTimeout = TimeSpan.Equals(TimeSpan.Zero, namedPipeConnectTimeout) ? TimeSpan.FromMilliseconds(100) : namedPipeConnectTimeout;
        DefaultHttpRequestHeaders = defaultHttpRequestHeaders ?? new Dictionary<string, string>();
        SocketConfiguration = socketConfiguration ?? SocketConnectionConfiguration.Default;
        ConfigureHandler = configureHandler;
    }

    /// <summary>
    /// Gets the collection of default HTTP request headers.
    /// </summary>
    public IReadOnlyDictionary<string, string> DefaultHttpRequestHeaders { get; }

    public Uri EndpointBaseUri { get; }

    public Credentials Credentials { get; }

    public TimeSpan DefaultTimeout { get; }

    public TimeSpan NamedPipeConnectTimeout { get; }

    /// <summary>
    /// Gets the socket connection configuration applied to Unix domain socket and TCP connections.
    /// </summary>
    public SocketConnectionConfiguration SocketConfiguration { get; }

    /// <summary>
    /// Gets an optional callback that is invoked with the <see cref="HttpMessageHandler"/> after
    /// it is created by the handler factory. Use this to apply custom configurations to the handler.
    /// </summary>
    public Action<HttpMessageHandler> ConfigureHandler { get; }

    public DockerClient CreateClient(Version requestedApiVersion = null, ILogger logger = null)
    {
        var handlerFactory = EndpointBaseUri.Scheme.ToLowerInvariant() switch
        {
            "npipe" => NPipe.DockerHandlerFactory.Instance,
            "unix" => Unix.DockerHandlerFactory.Instance,
            "tcp" or "http" or "https" => NativeHttpEnabled
                ? NativeHttp.DockerHandlerFactory.Instance
                : LegacyHttp.DockerHandlerFactory.Instance,
            _ => throw new NotSupportedException($"The URI scheme '{EndpointBaseUri.Scheme}' is not supported.")
        };

        return CreateClient(requestedApiVersion, handlerFactory, logger);
    }

    public DockerClient CreateClient(Version requestedApiVersion, IDockerHandlerFactory handlerFactory, ILogger logger = null)
    {
        if (handlerFactory == null)
        {
            throw new ArgumentNullException(nameof(handlerFactory));
        }

        return new DockerClient(this, requestedApiVersion, handlerFactory, logger);
    }

    public void Dispose()
    {
        Credentials.Dispose();
    }

    private static Uri GetLocalDockerEndpoint()
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        return isWindows ? new Uri("npipe://./pipe/docker_engine") : new Uri("unix:/var/run/docker.sock");
    }
}

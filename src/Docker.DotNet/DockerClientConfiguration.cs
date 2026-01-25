namespace Docker.DotNet;

using System;

public class DockerClientConfiguration : IDisposable
{
    public DockerClientConfiguration(
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        TimeSpan socketConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null,
        SocketConnectionConfiguration socketConfiguration = null)
        : this(GetLocalDockerEndpoint(), credentials, defaultTimeout, namedPipeConnectTimeout, socketConnectTimeout, defaultHttpRequestHeaders, socketConfiguration)
    {
    }

    public DockerClientConfiguration(
        Uri endpoint,
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        TimeSpan socketConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null,
        SocketConnectionConfiguration socketConfiguration = null)
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
        SocketConnectTimeout = TimeSpan.Equals(TimeSpan.Zero, socketConnectTimeout) ? TimeSpan.FromSeconds(30) : socketConnectTimeout;
        DefaultHttpRequestHeaders = defaultHttpRequestHeaders ?? new Dictionary<string, string>();
        SocketConnectionConfiguration = socketConfiguration ?? SocketConnectionConfiguration.Default;
    }

    /// <summary>
    /// Gets the Docker endpoint base URI.
    /// </summary>
    public Uri EndpointBaseUri { get; }

    /// <summary>
    /// Gets the credentials used for authentication.
    /// </summary>
    public Credentials Credentials { get; }

    /// <summary>
    /// Gets the default timeout for API requests.
    /// </summary>
    public TimeSpan DefaultTimeout { get; }

    /// <summary>
    /// Gets the timeout for named pipe connections (Windows).
    /// </summary>
    public TimeSpan NamedPipeConnectTimeout { get; }

    /// <summary>
    /// Gets the timeout for Unix domain socket connections.
    /// </summary>
    public TimeSpan SocketConnectTimeout { get; }

    /// <summary>
    /// Gets the socket configuration options for connection handling.
    /// These settings help improve proxy compatibility and connection reliability.
    /// </summary>
    public SocketConnectionConfiguration SocketConnectionConfiguration { get; }

    /// <summary>
    /// Gets the collection of default HTTP request headers.
    /// </summary>
    public IReadOnlyDictionary<string, string> DefaultHttpRequestHeaders { get; }

    public DockerClient CreateClient(Version requestedApiVersion = null, ILogger logger = null)
    {
        return new DockerClient(this, requestedApiVersion, logger);
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
namespace Docker.DotNet;

using System;

public class DockerClientConfiguration : IDisposable
{
    public DockerClientConfiguration(
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        TimeSpan socketConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null)
        : this(GetLocalDockerEndpoint(), credentials, defaultTimeout, namedPipeConnectTimeout, socketConnectTimeout, defaultHttpRequestHeaders)
    {
    }

    public DockerClientConfiguration(
        Uri endpoint,
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        TimeSpan socketConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null)
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
    /// Gets the timeout for Unix domain socket connections.
    /// </summary>
    public TimeSpan SocketConnectTimeout { get; }

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
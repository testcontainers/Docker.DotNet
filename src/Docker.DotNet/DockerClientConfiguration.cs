namespace Docker.DotNet;

using System;

public class DockerClientConfiguration : IDisposable
{
    public DockerClientConfiguration(
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null,
        TimeSpan unixSocketConnectTimeout = default)
        : this(GetLocalDockerEndpoint(), credentials, defaultTimeout, namedPipeConnectTimeout, defaultHttpRequestHeaders, unixSocketConnectTimeout)
    {
    }

    public DockerClientConfiguration(
        Uri endpoint,
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null,
        TimeSpan unixSocketConnectTimeout = default)
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
        UnixSocketConnectTimeout = TimeSpan.Equals(TimeSpan.Zero, unixSocketConnectTimeout) ? TimeSpan.FromSeconds(30) : unixSocketConnectTimeout;
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
    /// Gets the timeout for connecting to Unix domain sockets.
    /// </summary>
    public TimeSpan UnixSocketConnectTimeout { get; }

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
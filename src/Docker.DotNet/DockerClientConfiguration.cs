namespace Docker.DotNet;

using System;

public class DockerClientConfiguration : IDisposable
{
    public DockerClientConfiguration(
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null)
        : this(GetLocalDockerEndpoint(), credentials, defaultTimeout, defaultHttpRequestHeaders)
    {
    }

    public DockerClientConfiguration(
        Uri endpoint,
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
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
        DefaultHttpRequestHeaders = defaultHttpRequestHeaders ?? new Dictionary<string, string>();
    }

    /// <summary>
    /// Gets the collection of default HTTP request headers.
    /// </summary>
    public IReadOnlyDictionary<string, string> DefaultHttpRequestHeaders { get; }

    public Uri EndpointBaseUri { get; }

    public Credentials Credentials { get; }

    public TimeSpan DefaultTimeout { get; }

    public DockerClient CreateClient(Version requestedApiVersion = null, ILogger logger = null)
    {
        var scheme = EndpointBaseUri.Scheme;
        if (scheme.Equals("http", StringComparison.OrdinalIgnoreCase) ||
            scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
        {
            scheme = "Http";
        }

        // Try to find a loaded handler factory that matches the scheme and Docker.DotNet
        var factoryType = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName.IndexOf("Docker.DotNet", StringComparison.OrdinalIgnoreCase) >= 0)
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t =>
                typeof(IDockerHandlerFactory).IsAssignableFrom(t) &&
                !t.IsInterface && !t.IsAbstract &&
                (t.Name.IndexOf(scheme, StringComparison.OrdinalIgnoreCase) >= 0 ||
                t.Namespace?.IndexOf(scheme, StringComparison.OrdinalIgnoreCase) >= 0)
            );

        if (factoryType == null)
        {
            throw new InvalidOperationException($"No Docker handler factory implementation found for scheme '{scheme}'. Please reference at least one handler package (e.g., NPipe, Unix, NativeHttp, LegacyHttp).");
        }

        var factory = (IDockerHandlerFactory)Activator.CreateInstance(factoryType);

        return new DockerClient(this, requestedApiVersion, factory, logger);
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
namespace Docker.DotNet;

using System;

public class DockerClientConfiguration : IDisposable
{
    public DockerClientConfiguration(
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
        IReadOnlyDictionary<string, string> defaultHttpRequestHeaders = null)
        : this(GetLocalDockerEndpoint(), credentials, defaultTimeout, namedPipeConnectTimeout, defaultHttpRequestHeaders)
    {
    }

    public DockerClientConfiguration(
        Uri endpoint,
        Credentials credentials = null,
        TimeSpan defaultTimeout = default,
        TimeSpan namedPipeConnectTimeout = default,
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

    public DockerClient CreateClient(System.Version requestedApiVersion = null, ILogger logger = null)
    {
        var scheme = EndpointBaseUri.Scheme;
        if (scheme.Equals("http", StringComparison.OrdinalIgnoreCase) ||
            scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
        {
            scheme = "Http";
        }

        // Try to find a handler factory assembly in base directory that matches the scheme and Docker.DotNet
        var filenameOfFactoryAssembly = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
        .FirstOrDefault(a =>
            a.ToLower().Contains("Docker.DotNet".ToLower())
            && a.ToLower().Contains(scheme.ToLower()));

        if (filenameOfFactoryAssembly == null)
        {
            throw new InvalidOperationException($"No Docker handler factory assembly found for scheme '{scheme}'. Please reference at least one handler package (e.g., NPipe, Unix, NativeHttp, LegacyHttp).");
        }

        var factoryAssembly = Assembly.LoadFile(filenameOfFactoryAssembly);

        var factoryType = factoryAssembly.GetTypes().FirstOrDefault(t =>
            typeof(IDockerHandlerFactory).IsAssignableFrom(t) &&
            !t.IsInterface && !t.IsAbstract
        );

        if (factoryType == null)
        {
            throw new InvalidOperationException($"No Docker handler factory implementation found for scheme '{scheme}' in assembly '{factoryAssembly.FullName}'.");
        }

        var factory = (IDockerHandlerFactory)Activator.CreateInstance(factoryType);

        return new DockerClient(this, requestedApiVersion, factory, logger);
    }

    public DockerClient CreateClient(System.Version requestedApiVersion, IDockerHandlerFactory handlerFactory, ILogger logger = null)
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
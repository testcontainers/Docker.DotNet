namespace Docker.DotNet;

using System;

/// <summary>
/// Builds a <see cref="DockerClient"/>.
/// </summary>
public class DockerClientBuilder
{
    private static readonly bool NativeHttpEnabled = Environment.GetEnvironmentVariable("DOCKER_DOTNET_NATIVE_HTTP_ENABLED") == "1";

    private static readonly Uri LocalDockerEndpoint = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new Uri("npipe://./pipe/docker_engine") : new Uri("unix:/var/run/docker.sock");

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder"/> class.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>npipe://./pipe/docker_engine</c> on Windows and <c>unix:/var/run/docker.sock</c> on Linux/macOS.
    /// </remarks>
    public DockerClientBuilder()
    {
        _ = WithEndpoint(LocalDockerEndpoint);
    }

    /// <summary>
    /// Gets the client options.
    /// </summary>
    protected ClientOptions ClientOptions { get; private set; } = new();

    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected ILogger Logger { get; private set; } = NullLogger.Instance;

    /// <summary>
    /// Sets the Docker Engine API version to request.
    /// </summary>
    /// <param name="version">The requested API version.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithApiVersion(Version version)
    {
        ClientOptions = ClientOptions with { ApiVersion = version };
        return this;
    }

    /// <summary>
    /// Sets the Docker endpoint to connect to.
    /// </summary>
    /// <param name="endpoint">The endpoint URI.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithEndpoint(Uri endpoint)
    {
        ClientOptions = ClientOptions with { Endpoint = endpoint };
        return this;
    }

    /// <summary>
    /// Sets the authentication provider used to configure the HTTP handler.
    /// </summary>
    /// <param name="authProvider">The authentication provider.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithAuthProvider(IAuthProvider authProvider)
    {
        ClientOptions = ClientOptions with { AuthProvider = authProvider };
        return this;
    }

    /// <summary>
    /// Adds or replaces a default HTTP header applied to each request.
    /// </summary>
    /// <param name="name">The header name.</param>
    /// <param name="value">The header value.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithHeader(string name, string value)
    {
        var merged = new Dictionary<string, string>();

        foreach (var kvp in ClientOptions.Headers)
        {
            merged[kvp.Key] = kvp.Value;
        }

        merged[name] = value;

        ClientOptions = ClientOptions with { Headers = merged };
        return this;
    }

    /// <summary>
    /// Adds or replaces default HTTP headers applied to each request.
    /// </summary>
    /// <param name="headers">The headers to apply.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithHeaders(IReadOnlyDictionary<string, string> headers)
    {
        var merged = new Dictionary<string, string>();

        foreach (var kvp in ClientOptions.Headers)
        {
            merged[kvp.Key] = kvp.Value;
        }

        foreach (var kvp in headers)
        {
            merged[kvp.Key] = kvp.Value;
        }

        ClientOptions = ClientOptions with { Headers = merged };
        return this;
    }

    /// <summary>
    /// Sets the maximum time to wait for an HTTP request to complete.
    /// </summary>
    /// <param name="timeout">The request timeout.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithTimeout(TimeSpan timeout)
    {
        ClientOptions = ClientOptions with { Timeout = timeout };
        return this;
    }

    /// <summary>
    /// Sets the logger.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithLogger(ILogger logger)
    {
        Logger = logger;
        return this;
    }

    /// <summary>
    /// Selects the legacy HTTP transport.
    /// </summary>
    /// <param name="transportOptions">The legacy HTTP transport options.</param>
    /// <returns>A typed builder that uses the legacy HTTP transport.</returns>
    public DockerClientBuilder<LegacyHttpTransportOptions> WithTransportOptions(LegacyHttpTransportOptions transportOptions)
    {
        return new DockerClientBuilder<LegacyHttpTransportOptions>(LegacyHttp.DockerHandlerFactory.Instance, transportOptions);
    }

    /// <summary>
    /// Selects the native HTTP transport.
    /// </summary>
    /// <param name="transportOptions">The native HTTP transport options.</param>
    /// <returns>A typed builder that uses the native HTTP transport.</returns>
    public DockerClientBuilder<NativeHttpTransportOptions> WithTransportOptions(NativeHttpTransportOptions transportOptions)
    {
        return new DockerClientBuilder<NativeHttpTransportOptions>(NativeHttp.DockerHandlerFactory.Instance, transportOptions);
    }

    /// <summary>
    /// Selects the Windows named pipe transport.
    /// </summary>
    /// <param name="transportOptions">The named pipe transport options.</param>
    /// <returns>A typed builder that uses the named pipe transport.</returns>
    public DockerClientBuilder<NPipeTransportOptions> WithTransportOptions(NPipeTransportOptions transportOptions)
    {
        return new DockerClientBuilder<NPipeTransportOptions>(NPipe.DockerHandlerFactory.Instance, transportOptions);
    }

    /// <summary>
    /// Selects the Unix domain socket transport.
    /// </summary>
    /// <param name="transportOptions">The Unix socket transport options.</param>
    /// <returns>A typed builder that uses the Unix socket transport.</returns>
    public DockerClientBuilder<UnixSocketTransportOptions> WithTransportOptions(UnixSocketTransportOptions transportOptions)
    {
        return new DockerClientBuilder<UnixSocketTransportOptions>(Unix.DockerHandlerFactory.Instance, transportOptions);
    }

    /// <summary>
    /// Builds a <see cref="DockerClient"/> using the configured options.
    /// </summary>
    /// <remarks>
    /// If no transport is explicitly selected, the transport is chosen from the endpoint URI scheme.
    /// </remarks>
    /// <returns>A configured <see cref="DockerClient"/> instance.</returns>
    public virtual DockerClient Build()
    {
        var scheme = ClientOptions.Endpoint.Scheme;

        IDockerHandlerFactory transportFactory = scheme.ToLowerInvariant() switch
        {
            "npipe" => NPipe.DockerHandlerFactory.Instance,
            "unix" => Unix.DockerHandlerFactory.Instance,
            "tcp" or "http" or "https" => NativeHttpEnabled
                ? NativeHttp.DockerHandlerFactory.Instance
                : LegacyHttp.DockerHandlerFactory.Instance,
            _ => throw new NotSupportedException($"The URI scheme '{scheme}' is not supported.")
        };

        var (handler, endpoint) = transportFactory.CreateHandler(ClientOptions, Logger);
        return new DockerClient(transportFactory, handler, ClientOptions, endpoint, Logger);
    }
}
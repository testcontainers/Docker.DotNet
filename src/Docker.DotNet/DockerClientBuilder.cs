namespace Docker.DotNet;

using System;

/// <summary>
/// Builds a <see cref="DockerClient"/>.
/// </summary>
public class DockerClientBuilder
{
    private static readonly bool NativeHttpEnabled = Environment.GetEnvironmentVariable("DOCKER_DOTNET_NATIVE_HTTP_ENABLED") == "1";

    private readonly DockerConfig _dockerConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder"/> class.
    /// </summary>
    /// <remarks>
    /// Resolves the Docker endpoint using the same precedence as the <c>docker</c> CLI.
    ///
    /// First, the value of <c>DOCKER_HOST</c> is used when it is set.
    ///
    /// If no host is specified, the active Docker context is used. The context is
    /// determined from <c>DOCKER_CONTEXT</c> or from <c>currentContext</c> in
    /// <c>~/.docker/config.json</c>.
    ///
    /// When neither a host nor a context is configured, the platform default
    /// endpoint is used: <c>unix:///var/run/docker.sock</c> on Linux and macOS
    /// and <c>npipe://./pipe/docker_engine</c> on Windows.
    /// </remarks>
    public DockerClientBuilder()
        : this(DockerConfig.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder"/> class.
    /// </summary>
    /// <param name="dockerConfig">The Docker config to preserve.</param>
    public DockerClientBuilder(
        DockerConfig dockerConfig)
        : this(dockerConfig, new ClientOptions { Endpoint = dockerConfig.GetEndpoint() }, NullLogger.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder"/> class.
    /// </summary>
    /// <param name="clientOptions">The client options to preserve.</param>
    /// <param name="logger">The logger to preserve.</param>
    protected DockerClientBuilder(
        ClientOptions clientOptions,
        ILogger logger)
        : this(DockerConfig.Instance, clientOptions, logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder"/> class.
    /// </summary>
    /// <param name="dockerConfig">The Docker config to preserve.</param>
    /// <param name="clientOptions">The client options to preserve.</param>
    /// <param name="logger">The logger to preserve.</param>
    protected DockerClientBuilder(
        DockerConfig dockerConfig,
        ClientOptions clientOptions,
        ILogger logger)
    {
        _dockerConfig = dockerConfig;
        ClientOptions = clientOptions;
        Logger = logger;
    }

    /// <summary>
    /// Gets the client options.
    /// </summary>
    protected ClientOptions ClientOptions { get; private set; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    protected ILogger Logger { get; private set; }

    /// <summary>
    /// Sets the Docker Engine API version to request.
    /// </summary>
    /// <param name="version">The requested API version.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithApiVersion(Version? version)
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
    /// Sets the Docker endpoint declared by the named Docker context.
    /// </summary>
    /// <remarks>
    /// Reads the endpoint from <c>~/.docker/contexts/meta/&lt;sha256(name)&gt;/meta.json</c>.
    /// </remarks>
    /// <param name="contextName">The context name (e.g. <c>desktop-linux</c>).</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithContext(string contextName)
    {
        return WithEndpoint(_dockerConfig.GetEndpoint(contextName));
    }

    /// <summary>
    /// Sets the authentication provider used to configure the HTTP handler.
    /// </summary>
    /// <param name="authProvider">The authentication provider.</param>
    /// <returns>The builder instance.</returns>
    public DockerClientBuilder WithAuthProvider(IAuthProvider? authProvider)
    {
        var nonNullableAuthProvider = authProvider ?? NoopAuthProvider.Instance;

        ClientOptions = ClientOptions with { AuthProvider = nonNullableAuthProvider };
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
        return new DockerClientBuilder<LegacyHttpTransportOptions>(LegacyHttp.DockerHandlerFactory.Instance, transportOptions, _dockerConfig, ClientOptions, Logger);
    }

    /// <summary>
    /// Selects the native HTTP transport.
    /// </summary>
    /// <param name="transportOptions">The native HTTP transport options.</param>
    /// <returns>A typed builder that uses the native HTTP transport.</returns>
    public DockerClientBuilder<NativeHttpTransportOptions> WithTransportOptions(NativeHttpTransportOptions transportOptions)
    {
        return new DockerClientBuilder<NativeHttpTransportOptions>(NativeHttp.DockerHandlerFactory.Instance, transportOptions, _dockerConfig, ClientOptions, Logger);
    }

    /// <summary>
    /// Selects the Windows named pipe transport.
    /// </summary>
    /// <param name="transportOptions">The named pipe transport options.</param>
    /// <returns>A typed builder that uses the named pipe transport.</returns>
    public DockerClientBuilder<NPipeTransportOptions> WithTransportOptions(NPipeTransportOptions transportOptions)
    {
        return new DockerClientBuilder<NPipeTransportOptions>(NPipe.DockerHandlerFactory.Instance, transportOptions, _dockerConfig, ClientOptions, Logger);
    }

    /// <summary>
    /// Selects the Unix domain socket transport.
    /// </summary>
    /// <param name="transportOptions">The Unix socket transport options.</param>
    /// <returns>A typed builder that uses the Unix socket transport.</returns>
    public DockerClientBuilder<UnixSocketTransportOptions> WithTransportOptions(UnixSocketTransportOptions transportOptions)
    {
        return new DockerClientBuilder<UnixSocketTransportOptions>(Unix.DockerHandlerFactory.Instance, transportOptions, _dockerConfig, ClientOptions, Logger);
    }

    /// <summary>
    /// Selects custom transport by providing a handler factory and transport-specific options.
    /// </summary>
    /// <typeparam name="TTransportOptions">The type of transport options consumed by the custom handler factory.</typeparam>
    /// <param name="transportFactory">The custom transport handler factory.</param>
    /// <param name="transportOptions">The transport-specific options passed to the custom handler factory.</param>
    /// <returns>A typed builder that uses the provided custom transport.</returns>
    public DockerClientBuilder<TTransportOptions> WithTransportOptions<TTransportOptions>(IDockerHandlerFactory<TTransportOptions> transportFactory, TTransportOptions transportOptions)
    {
        return new DockerClientBuilder<TTransportOptions>(transportFactory, transportOptions, _dockerConfig, ClientOptions, Logger);
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
        var transportFactory = ResolveTransportFactory(ClientOptions.Endpoint.Scheme);

        var resolvedTransport = transportFactory.CreateHandler(ClientOptions, Logger);

        var authenticatedHandler = ClientOptions.AuthProvider.ConfigureHandler(resolvedTransport.Handler);

        return new DockerClient(authenticatedHandler, ClientOptions, resolvedTransport.EffectiveEndpoint, transportFactory);
    }

    /// <summary>
    /// Resolves the transport handler factory for the provided endpoint URI scheme.
    /// </summary>
    /// <param name="scheme">The endpoint URI scheme.</param>
    /// <returns>The selected transport handler factory.</returns>
    protected virtual IDockerHandlerFactory ResolveTransportFactory(string scheme)
    {
        return scheme.ToLowerInvariant() switch
        {
            "npipe" => NPipe.DockerHandlerFactory.Instance,
            "unix" => Unix.DockerHandlerFactory.Instance,
            "tcp" or "http" or "https" => NativeHttpEnabled
                ? NativeHttp.DockerHandlerFactory.Instance
                : LegacyHttp.DockerHandlerFactory.Instance,
            "ssh" => throw new SshDockerEndpointNotSupportedException(),
            _ => throw new NotSupportedException($"The URI scheme '{scheme}' is not supported.")
        };
    }
}
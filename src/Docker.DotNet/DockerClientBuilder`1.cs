namespace Docker.DotNet;

/// <summary>
/// Builds a <see cref="DockerClient"/> using an explicitly selected transport handler.
/// </summary>
/// <typeparam name="TTransportOptions">The transport options type.</typeparam>
public sealed class DockerClientBuilder<TTransportOptions> : DockerClientBuilder
{
    private readonly IDockerHandlerFactory<TTransportOptions> _transportFactory;

    private readonly TTransportOptions _transportOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder{TTransportOptions}"/> class.
    /// </summary>
    /// <param name="transportFactory">The transport handler factory.</param>
    /// <param name="transportOptions">The transport options.</param>
    public DockerClientBuilder(
        IDockerHandlerFactory<TTransportOptions> transportFactory,
        TTransportOptions transportOptions)
        : base()
    {
        _transportFactory = transportFactory;
        _transportOptions = transportOptions;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder{TTransportOptions}"/> class.
    /// </summary>
    /// <param name="transportFactory">The transport handler factory.</param>
    /// <param name="transportOptions">The transport options.</param>
    /// <param name="dockerConfig">The Docker config to preserve.</param>
    /// <param name="clientOptions">The client options to preserve.</param>
    /// <param name="logger">The logger to preserve.</param>
    public DockerClientBuilder(
        IDockerHandlerFactory<TTransportOptions> transportFactory,
        TTransportOptions transportOptions,
        DockerConfig dockerConfig,
        ClientOptions clientOptions,
        ILogger logger)
        : base(dockerConfig, clientOptions, logger)
    {
        _transportFactory = transportFactory;
        _transportOptions = transportOptions;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerClientBuilder{TTransportOptions}"/> class.
    /// </summary>
    /// <param name="transportFactory">The transport handler factory.</param>
    /// <param name="transportOptions">The transport options.</param>
    /// <param name="clientOptions">The client options to preserve.</param>
    /// <param name="logger">The logger to preserve.</param>
    public DockerClientBuilder(
        IDockerHandlerFactory<TTransportOptions> transportFactory,
        TTransportOptions transportOptions,
        ClientOptions clientOptions,
        ILogger logger)
        : this(transportFactory, transportOptions, DockerConfig.Instance, clientOptions, logger)
    {
    }

    /// <summary>
    /// Builds a <see cref="DockerClient"/> using the explicitly selected transport handler.
    /// </summary>
    /// <returns>A configured <see cref="DockerClient"/> instance.</returns>
    public override DockerClient Build()
    {
        var clientOptions = CreateResolvedClientOptions();

        var resolvedTransport = _transportFactory.CreateHandler(_transportOptions, clientOptions, Logger);

        var authenticatedHandler = clientOptions.AuthProvider.ConfigureHandler(resolvedTransport.Handler);

        return new DockerClient(authenticatedHandler, clientOptions, resolvedTransport.EffectiveEndpoint, _transportFactory);
    }
}
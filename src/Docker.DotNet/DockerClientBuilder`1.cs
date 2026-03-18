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
        : base(clientOptions, logger)
    {
        _transportFactory = transportFactory;
        _transportOptions = transportOptions;
    }

    /// <summary>
    /// Builds a <see cref="DockerClient"/> using the explicitly selected transport handler.
    /// </summary>
    /// <returns>A configured <see cref="DockerClient"/> instance.</returns>
    public override DockerClient Build()
    {
        var resolvedTransport = _transportFactory.CreateHandler(_transportOptions, ClientOptions, Logger);

        var authenticatedHandler = ClientOptions.AuthProvider.ConfigureHandler(resolvedTransport.Handler);

        return new DockerClient(authenticatedHandler, ClientOptions, resolvedTransport.EffectiveEndpoint, _transportFactory);
    }
}
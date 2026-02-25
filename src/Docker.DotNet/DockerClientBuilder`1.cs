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
    /// Builds a <see cref="DockerClient"/> using the explicitly selected transport handler.
    /// </summary>
    /// <returns>A configured <see cref="DockerClient"/> instance.</returns>
    public override DockerClient Build()
    {
        var (handler, endpoint) = _transportFactory.CreateHandler(_transportOptions, ClientOptions, Logger);

        var clientOptions = ClientOptions with { Endpoint = endpoint };

        var authenticatedHandler = clientOptions.AuthProvider.ConfigureHandler(handler);

        return new DockerClient(authenticatedHandler, clientOptions, _transportFactory, Logger);
    }
}
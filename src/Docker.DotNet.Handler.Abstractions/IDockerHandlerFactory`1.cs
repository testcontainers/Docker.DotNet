namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// Creates HTTP handlers using transport-specific options.
/// </summary>
/// <typeparam name="TTransportOptions">The transport options type.</typeparam>
public interface IDockerHandlerFactory<in TTransportOptions> : IDockerHandlerFactory
{
    /// <summary>
    /// Creates a handler and normalized endpoint URI for the provided transport and client options.
    /// </summary>
    /// <param name="transportOptions">The transport-specific options.</param>
    /// <param name="clientOptions">The client options.</param>
    /// <param name="logger">The logger instance.</param>
    /// <returns>A tuple containing the configured handler and normalized endpoint URI.</returns>
    Tuple<HttpMessageHandler, Uri> CreateHandler(TTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger);
}
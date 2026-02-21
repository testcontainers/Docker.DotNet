namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// Creates HTTP handlers used to communicate with the Docker Engine API.
/// </summary>
public interface IDockerHandlerFactory : IStreamHijacker
{
    /// <summary>
    /// Creates a handler and normalized endpoint URI for the provided client options.
    /// </summary>
    /// <param name="clientOptions">The client options.</param>
    /// <param name="logger">The logger instance.</param>
    /// <returns>A tuple containing the configured handler and normalized endpoint URI.</returns>
    Tuple<HttpMessageHandler, Uri> CreateHandler(ClientOptions clientOptions, ILogger logger);
}
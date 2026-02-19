namespace Docker.DotNet.Handler.Abstractions;

public interface IDockerHandlerFactory<in TTransportOptions> : IDockerHandlerFactory
{
    Tuple<HttpMessageHandler, Uri> CreateHandler(TTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger);
}
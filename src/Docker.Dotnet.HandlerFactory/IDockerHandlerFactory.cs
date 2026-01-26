namespace Docker.DotNet.HandlerFactory;

public interface IDockerHandlerFactory
{
    Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger);
}

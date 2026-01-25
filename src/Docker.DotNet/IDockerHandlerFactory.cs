namespace Docker.DotNet;

public interface IDockerHandlerFactory
{
    Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, DockerClientConfiguration configuration, ILogger logger);
}
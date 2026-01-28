namespace Docker.DotNet.Handler.Abstractions;

public interface IDockerHandlerFactory : IStreamHijacker
{
    Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger);
}
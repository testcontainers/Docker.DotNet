namespace Docker.DotNet.Handler.Abstractions;

public interface IDockerHandlerFactory : IStreamHijacker
{
    [Obsolete("Use the CreateHandler(ClientOptions, ILogger) or CreateHandler(TTransportOptions, ClientOptions, ILogger) overload instead.")]
    Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger);

    Tuple<HttpMessageHandler, Uri> CreateHandler(ClientOptions clientOptions, ILogger logger);
}
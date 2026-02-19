namespace Docker.DotNet.Unix;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<UnixSocketTransportOptions>
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<UnixSocketTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        var clientOptions = new ClientOptions { Endpoint = uri };
        return CreateHandler(clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(ClientOptions clientOptions, ILogger logger)
    {
        return CreateHandler(new UnixSocketTransportOptions(), clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(UnixSocketTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
        var uri = clientOptions.Endpoint;

        var socketName = uri.Segments.Last();
        var socketPath = uri.LocalPath;
        uri = new UriBuilder(Uri.UriSchemeHttp, socketName).Uri;

        var socketOpener = new ManagedHandler.SocketOpener(async (_, _, _) =>
        {
            var endpoint = new UnixDomainSocketEndPoint(socketPath);

            var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

            await socket.ConnectAsync(endpoint)
                .ConfigureAwait(false);

            return socket;
        });

        return new Tuple<HttpMessageHandler, Uri>(new ManagedHandler(socketOpener, logger), uri);
    }

    public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        if (content is not HttpConnectionResponseContent hijackable)
        {
            throw new NotSupportedException("The content type is not supported for stream hijacking.");
        }

        return Task.FromResult(hijackable.HijackStream());
    }
}
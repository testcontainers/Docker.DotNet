namespace Docker.DotNet.Unix;

public class HandlerFactory : IDockerHandlerFactory
{
    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
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
}
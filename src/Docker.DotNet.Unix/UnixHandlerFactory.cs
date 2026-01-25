namespace Docker.DotNet.Unix;

public class UnixHandlerFactory : IDockerHandlerFactory
{
    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, DockerClientConfiguration configuration, ILogger logger)
    {
        var socketPath = uri.LocalPath;
        uri = new UriBuilder(uri) { Scheme = Uri.UriSchemeHttp, Host = uri.Segments.Last() }.Uri;

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
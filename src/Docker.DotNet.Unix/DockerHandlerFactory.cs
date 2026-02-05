namespace Docker.DotNet.Unix;

public sealed class DockerHandlerFactory : IDockerHandlerFactory
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory Instance { get; } = new DockerHandlerFactory();

    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        var socketName = uri.Segments.Last();
        var socketPath = uri.LocalPath;
        uri = new UriBuilder(Uri.UriSchemeHttp, socketName).Uri;

        var socketConfiguration = configuration.SocketConfiguration ?? SocketConnectionConfiguration.Default;

        var socketOpener = new ManagedHandler.SocketOpener(async (_, _, cancellationToken) =>
        {
            var endpoint = new UnixDomainSocketEndPoint(socketPath);

            var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

            try
            {
                socketConfiguration.Apply(socket);

#if NET5_0_OR_GREATER
                await socket.ConnectAsync(endpoint, cancellationToken)
                    .ConfigureAwait(false);
#else
                await socket.ConnectAsync(endpoint)
                    .ConfigureAwait(false);
#endif

                return socket;
            }
            catch
            {
                socket.Dispose();
                throw;
            }
        });

        var handler = new ManagedHandler(socketOpener, logger);

        // Unix domain sockets are local connections; proxy resolution is not applicable.
        handler.UseProxy = false;

        return new Tuple<HttpMessageHandler, Uri>(handler, uri);
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

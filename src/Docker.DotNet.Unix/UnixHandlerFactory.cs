namespace Docker.DotNet.Unix
{
    public class UnixHandlerFactory : IDockerHandlerFactory
    {
        public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, DockerClientConfiguration configuration, ILogger logger)
        {
            var pipeString = uri.LocalPath;
            uri = new UriBuilder("http", uri.Segments.Last()).Uri;
            return new Tuple<HttpMessageHandler, Uri>(
                new Microsoft.Net.Http.Client.ManagedHandler(async (host, port, cancellationToken) =>
                {
                    var sock = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
                    await sock.ConnectAsync(new UnixDomainSocketEndPoint(pipeString)).ConfigureAwait(false);
                    return sock;
                }, logger),
                uri
            );
        }
    }
}

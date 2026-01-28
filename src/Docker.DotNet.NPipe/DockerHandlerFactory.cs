namespace Docker.DotNet.NPipe;

public class DockerHandlerFactory : IDockerHandlerFactory
{
    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        if (configuration.Credentials.IsTlsCredentials())
        {
            throw new NotSupportedException("TLS is not supported over npipe.");
        }

        var segments = uri.Segments;

        if (segments.Length != 3 || !"pipe/".Equals(segments[1], StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The endpoint is not a valid npipe URI.");
        }

        var pipeName = uri.Segments[2];

        var serverName = "localhost".Equals(uri.Host, StringComparison.OrdinalIgnoreCase) ? "." : uri.Host;
        uri = new UriBuilder(Uri.UriSchemeHttp, pipeName).Uri;

        var streamOpener = new ManagedHandler.StreamOpener(async (_, _, cancellationToken) =>
        {
            var clientStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            var dockerStream = new DockerPipeStream(clientStream);

#if NETSTANDARD
            var namedPipeConnectTimeout = (int)configuration.NamedPipeConnectTimeout.TotalMilliseconds;
#else
            var namedPipeConnectTimeout = configuration.NamedPipeConnectTimeout;
#endif

            await clientStream.ConnectAsync(namedPipeConnectTimeout, cancellationToken)
                .ConfigureAwait(false);

            return dockerStream;
        });

        return new Tuple<HttpMessageHandler, Uri>(new ManagedHandler(streamOpener, logger), uri);
    }
}
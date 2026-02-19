namespace Docker.DotNet.NPipe;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<NPipeTransportOptions>
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<NPipeTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        var transportOptions = new NPipeTransportOptions { ConnectTimeout = configuration.NamedPipeConnectTimeout };
        var clientOptions = new ClientOptions { Endpoint = uri, AuthProvider = new DelegateAuthProvider(configuration) };
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(ClientOptions clientOptions, ILogger logger)
    {
        return CreateHandler(new NPipeTransportOptions(), clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(NPipeTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
        if (clientOptions.AuthProvider.TlsEnabled)
        {
            throw new NotSupportedException("TLS is not supported over npipe.");
        }

        var uri = clientOptions.Endpoint;

        var segments = uri.Segments;

        if (segments.Length != 3 || !"pipe/".Equals(segments[1], StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The endpoint is not a npipe URI.");
        }

        var pipeName = uri.Segments[2];

        var serverName = "localhost".Equals(uri.Host, StringComparison.OrdinalIgnoreCase) ? "." : uri.Host;
        uri = new UriBuilder(Uri.UriSchemeHttp, pipeName).Uri;

        var streamOpener = new ManagedHandler.StreamOpener(async (_, _, cancellationToken) =>
        {
            var clientStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            var dockerStream = new DockerPipeStream(clientStream);

#if NETSTANDARD
            var namedPipeConnectTimeout = (int)transportOptions.ConnectTimeout.TotalMilliseconds;
#else
            var namedPipeConnectTimeout = transportOptions.ConnectTimeout;
#endif

            await clientStream.ConnectAsync(namedPipeConnectTimeout, cancellationToken)
                .ConfigureAwait(false);

            return dockerStream;
        });

        return new Tuple<HttpMessageHandler, Uri>(new ManagedHandler(streamOpener, logger), uri);
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
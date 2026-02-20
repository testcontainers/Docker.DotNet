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
        var transportOptions = new UnixSocketTransportOptions();
        Validate(transportOptions, clientOptions);
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(UnixSocketTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
        Validate(transportOptions, clientOptions);

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

        var handler = new ManagedHandler(socketOpener, logger);
        transportOptions.ConfigureHandler(handler);

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

    private static void Validate(UnixSocketTransportOptions _, ClientOptions clientOptions)
    {
        if (clientOptions.Endpoint is null)
        {
            throw new ArgumentNullException(nameof(clientOptions), "ClientOptions.Endpoint must be set.");
        }

        var scheme = clientOptions.Endpoint.Scheme;

        if (!string.Equals(scheme, "unix", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(UnixSocketTransportOptions)}' can only be used with endpoint scheme 'unix', but '{scheme}' was provided.");
        }
    }
}
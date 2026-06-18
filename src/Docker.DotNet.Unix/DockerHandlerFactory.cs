namespace Docker.DotNet.Unix;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<UnixSocketTransportOptions>
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<UnixSocketTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public ResolvedTransport CreateHandler(ResolvedClientOptions clientOptions, ILogger logger)
    {
        var transportOptions = new UnixSocketTransportOptions();
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public ResolvedTransport CreateHandler(UnixSocketTransportOptions transportOptions, ResolvedClientOptions clientOptions, ILogger logger)
    {
        if (clientOptions is null)
        {
            throw new ArgumentNullException(nameof(clientOptions));
        }

        var uri = clientOptions.Endpoint;

        Validate(uri);

        var socketName = uri.Segments.Last();
        var socketPath = uri.LocalPath;
        uri = new UriBuilder(Uri.UriSchemeHttp, socketName).Uri;

        var socketOpener = new ManagedHandler.SocketOpener(async (_, _, cancellationToken) =>
        {
            var endpoint = new UnixDomainSocketEndPoint(socketPath);

            var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

#if NET
            await socket.ConnectAsync(endpoint, cancellationToken)
                .ConfigureAwait(false);
#else
            await socket.ConnectAsync(endpoint)
                .ConfigureAwait(false);
#endif

            return socket;
        });

        var handler = new ManagedHandler(socketOpener, logger);
        transportOptions.ConfigureHandler(handler);

        return new ResolvedTransport(handler, uri);
    }

    public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        if (content is not HttpConnectionResponseContent hijackable)
        {
            throw new NotSupportedException("The content type is not supported for stream hijacking.");
        }

        return Task.FromResult(hijackable.HijackStream());
    }

    private static void Validate(Uri endpoint)
    {
        var scheme = endpoint.Scheme;

        if (!string.Equals(scheme, "unix", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(UnixSocketTransportOptions)}' can only be used with endpoint scheme 'unix', but '{scheme}' was provided.");
        }
    }
}
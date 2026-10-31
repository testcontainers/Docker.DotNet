namespace Docker.DotNet.NativeHttp;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<NativeHttpTransportOptions>
{
    private static readonly TimeSpan PooledConnectionLifetime = TimeSpan.FromMinutes(5);

    private static readonly TimeSpan PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);

    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<NativeHttpTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public ResolvedTransport CreateHandler(ResolvedClientOptions clientOptions, ILogger logger)
    {
        var transportOptions = new NativeHttpTransportOptions();
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public ResolvedTransport CreateHandler(NativeHttpTransportOptions transportOptions, ResolvedClientOptions clientOptions, ILogger logger)
    {
        if (clientOptions is null)
        {
            throw new ArgumentNullException(nameof(clientOptions));
        }

        var uri = clientOptions.Endpoint;

        Validate(uri);

#if NET
        var handler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = PooledConnectionLifetime,
            PooledConnectionIdleTimeout = PooledConnectionIdleTimeout,
            PlaintextStreamFilter = TrackingNetworkStream.EnableTrackingAsync,
        };

        if (IsUnixSocket(uri))
        {
            var socketName = uri.Segments.Last();
            var socketPath = uri.LocalPath;
            uri = new UriBuilder(Uri.UriSchemeHttp, socketName).Uri;

            // Proxies do not apply to Unix domain sockets.
            handler.UseProxy = false;
            handler.ConnectCallback = (context, cancellationToken) => ConnectUnixSocketAsync(context, socketPath, cancellationToken);
        }
        else
        {
            uri = CreateHttpUri(uri, clientOptions);
            handler.ConnectCallback = ConnectTcpSocketAsync;
        }

        transportOptions.ConfigureHandler(handler);
#else
        uri = CreateHttpUri(uri, clientOptions);
        var handler = new HttpClientHandler();
        transportOptions.ConfigureHandler(handler);
#endif

        return new ResolvedTransport(handler, uri);
    }

    public async Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        var stream = await content.ReadAsStreamAsync()
            .ConfigureAwait(false);

        return new WriteClosableStreamWrapper(stream, HijackScope.Current?.Socket);
    }

    private static void Validate(Uri endpoint)
    {
        var scheme = endpoint.Scheme;

#if NET
        if (IsUnixSocket(endpoint))
        {
            return;
        }
#endif

        if (!string.Equals(scheme, "tcp", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
#if NET
            throw new InvalidOperationException($"The selected '{nameof(NativeHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', 'https', or 'unix', but '{scheme}' was provided.");
#else
            throw new InvalidOperationException($"The selected '{nameof(NativeHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', or 'https', but '{scheme}' was provided.");
#endif
        }
    }

#if NET
    private static bool IsUnixSocket(Uri endpoint)
    {
        return string.Equals(endpoint.Scheme, "unix", StringComparison.OrdinalIgnoreCase);
    }

    private static async ValueTask<Stream> ConnectUnixSocketAsync(SocketsHttpConnectionContext context, string socketPath, CancellationToken cancellationToken)
    {
        var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        try
        {
            await socket.ConnectAsync(new UnixDomainSocketEndPoint(socketPath), cancellationToken)
                .ConfigureAwait(false);

            return TrackingNetworkStream.Create(socket, context);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    private static async ValueTask<Stream> ConnectTcpSocketAsync(SocketsHttpConnectionContext context, CancellationToken cancellationToken)
    {
        var socket = new Socket(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };

        try
        {
            await socket.ConnectAsync(context.DnsEndPoint, cancellationToken)
                .ConfigureAwait(false);

            return TrackingNetworkStream.Create(socket, context);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }
#endif

    private static Uri CreateHttpUri(Uri endpoint, ResolvedClientOptions clientOptions)
    {
        var scheme = clientOptions.AuthProvider.TlsEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        return new UriBuilder(endpoint) { Scheme = scheme }.Uri;
    }
}
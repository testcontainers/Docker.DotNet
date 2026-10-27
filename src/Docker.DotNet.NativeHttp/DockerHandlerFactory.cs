namespace Docker.DotNet.NativeHttp;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<NativeHttpTransportOptions>
{
    private const int MaxConnectionsPerServer = 10;

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

        var endpoint = clientOptions.Endpoint;

        Validate(endpoint);

#if NET
        var handler = new SocketsHttpHandler
        {
            MaxConnectionsPerServer = MaxConnectionsPerServer,
            PooledConnectionLifetime = PooledConnectionLifetime,
            PooledConnectionIdleTimeout = PooledConnectionIdleTimeout,
        };

        Uri uri;

        if (IsUnixSocket(endpoint))
        {
            // Every connection goes to the socket, so the request URI only supplies the Host header, named after
            // the socket as the Unix transport names it.
            var socketPath = endpoint.LocalPath;
            uri = new UriBuilder(Uri.UriSchemeHttp, endpoint.Segments.Last()).Uri;
            handler.ConnectCallback = (_, cancellationToken) => ConnectUnixSocketAsync(socketPath, cancellationToken);
        }
        else
        {
            uri = CreateHttpUri(endpoint, clientOptions);
        }

        transportOptions.ConfigureHandler(handler);
#else
        var uri = CreateHttpUri(endpoint, clientOptions);
        var handler = new HttpClientHandler();
        transportOptions.ConfigureHandler(handler);
#endif

        return new ResolvedTransport(handler, uri);
    }

    public async Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        var stream = await content.ReadAsStreamAsync()
            .ConfigureAwait(false);

        return new WriteClosableStreamWrapper(stream);
    }

    private static Uri CreateHttpUri(Uri endpoint, ResolvedClientOptions clientOptions)
    {
        var scheme = clientOptions.AuthProvider.TlsEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        return new UriBuilder(endpoint) { Scheme = scheme }.Uri;
    }

#if NET
    private static bool IsUnixSocket(Uri endpoint)
    {
        return string.Equals(endpoint.Scheme, "unix", StringComparison.OrdinalIgnoreCase);
    }

    private static async ValueTask<Stream> ConnectUnixSocketAsync(string socketPath, CancellationToken cancellationToken)
    {
        var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        try
        {
            await socket.ConnectAsync(new UnixDomainSocketEndPoint(socketPath), cancellationToken)
                .ConfigureAwait(false);

            return new NetworkStream(socket, ownsSocket: true);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    private static void Validate(Uri endpoint)
    {
        var scheme = endpoint.Scheme;

        if (!string.Equals(scheme, "tcp", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
            && !IsUnixSocket(endpoint))
        {
            throw new InvalidOperationException($"The selected '{nameof(NativeHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', 'https', or 'unix', but '{scheme}' was provided.");
        }
    }
#else
    private static void Validate(Uri endpoint)
    {
        var scheme = endpoint.Scheme;

        // HttpClientHandler cannot connect to a Unix domain socket; that needs SocketsHttpHandler's ConnectCallback.
        if (!string.Equals(scheme, "tcp", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(NativeHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', or 'https' on this target framework, but '{scheme}' was provided.");
        }
    }
#endif
}

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

        var scheme = clientOptions.AuthProvider.TlsEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        var uri = new UriBuilder(endpoint) { Scheme = scheme }.Uri;

#if NET
        var handler = new SocketsHttpHandler
        {
            MaxConnectionsPerServer = MaxConnectionsPerServer,
            PooledConnectionLifetime = PooledConnectionLifetime,
            PooledConnectionIdleTimeout = PooledConnectionIdleTimeout,
        };

        transportOptions.ConfigureHandler(handler);
#else
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

    private static void Validate(Uri endpoint)
    {
        var scheme = endpoint.Scheme;

        if (!string.Equals(scheme, "tcp", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(NativeHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', or 'https', but '{scheme}' was provided.");
        }
    }
}
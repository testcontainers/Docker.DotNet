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

    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        var clientOptions = new ClientOptions { Endpoint = uri, AuthProvider = new DelegateAuthProvider(configuration) };
        return CreateHandler(clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(ClientOptions clientOptions, ILogger logger)
    {
        var transportOptions = new NativeHttpTransportOptions();
        Validate(transportOptions, clientOptions);
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(NativeHttpTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
        Validate(transportOptions, clientOptions);

        var scheme = clientOptions.AuthProvider.TlsEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        var uri = new UriBuilder(clientOptions.Endpoint) { Scheme = scheme }.Uri;

#if NET6_0_OR_GREATER
        var handler = new SocketsHttpHandler
        {
            MaxConnectionsPerServer = MaxConnectionsPerServer,
            PooledConnectionLifetime = PooledConnectionLifetime,
            PooledConnectionIdleTimeout = PooledConnectionIdleTimeout,
        };
#else
        var handler = new HttpClientHandler();
#endif

        return new Tuple<HttpMessageHandler, Uri>(handler, uri);
    }

    public async Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        var stream = await content.ReadAsStreamAsync()
            .ConfigureAwait(false);

        return new WriteClosableStreamWrapper(stream);
    }

    private static void Validate(NativeHttpTransportOptions _, ClientOptions clientOptions)
    {
        if (clientOptions.Endpoint is null)
        {
            throw new ArgumentNullException(nameof(clientOptions), "ClientOptions.Endpoint must be set.");
        }

        var scheme = clientOptions.Endpoint.Scheme;

        if (!string.Equals(scheme, "tcp", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(NativeHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', or 'https', but '{scheme}' was provided.");
        }
    }
}
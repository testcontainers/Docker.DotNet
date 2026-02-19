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
        return CreateHandler(new NativeHttpTransportOptions(), clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(NativeHttpTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
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
}
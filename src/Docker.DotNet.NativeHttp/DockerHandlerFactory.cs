namespace Docker.DotNet.NativeHttp;

public class DockerHandlerFactory : IDockerHandlerFactory
{
    private const int MaxConnectionsPerServer = 10;

    private static readonly TimeSpan PooledConnectionLifetime = TimeSpan.FromMinutes(5);

    private static readonly TimeSpan PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2);

    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        var scheme = configuration.Credentials.IsTlsCredentials() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        uri = new UriBuilder(uri) { Scheme = scheme }.Uri;

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
}
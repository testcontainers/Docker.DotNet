namespace Docker.DotNet.NativeHttp
{
    public class NativeHttpHandlerFactory : IDockerHandlerFactory
    {
        public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, DockerClientConfiguration configuration, ILogger logger)
        {

            var builder = new UriBuilder(uri)
            {
                Scheme = configuration.Credentials.IsTlsCredentials() ? "https" : "http"
            };
            uri = builder.Uri;

#if NET6_0_OR_GREATER
            return new Tuple<HttpMessageHandler, Uri>(
                new SocketsHttpHandler()
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
                    MaxConnectionsPerServer = 10
                },
                uri
            );
#else
            return new Tuple<HttpMessageHandler, Uri>(
                new HttpClientHandler(),
                uri
            );
#endif
        }
    }
}

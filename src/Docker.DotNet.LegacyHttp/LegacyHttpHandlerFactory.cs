namespace Docker.DotNet.LegacyHttp
{
    public class LegacyHttpHandlerFactory : IDockerHandlerFactory
    {
        public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, DockerClientConfiguration configuration, ILogger logger)
        {
            var builder = new UriBuilder(uri)
            {
                    Scheme = configuration.Credentials.IsTlsCredentials() ? "https" : "http"
            };
            uri = builder.Uri;
            return new Tuple<HttpMessageHandler, Uri>(
                new Microsoft.Net.Http.Client.ManagedHandler(logger),
                uri
            );
        }
    }
}

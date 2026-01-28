namespace Docker.DotNet.LegacyHttp;

public sealed class DockerHandlerFactory : IDockerHandlerFactory
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory Instance { get; } = new DockerHandlerFactory();

    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        var scheme = configuration.Credentials.IsTlsCredentials() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        uri = new UriBuilder(uri) { Scheme = scheme }.Uri;
        return new Tuple<HttpMessageHandler, Uri>(new ManagedHandler(logger), uri);
    }

    public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        if (content is not HttpConnectionResponseContent hijackable)
        {
            throw new NotSupportedException("Not supported content type for stream hijacking.");
        }

        return Task.FromResult(hijackable.HijackStream());
    }
}
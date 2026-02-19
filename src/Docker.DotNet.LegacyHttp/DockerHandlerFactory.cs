namespace Docker.DotNet.LegacyHttp;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<LegacyHttpTransportOptions>
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<LegacyHttpTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public Tuple<HttpMessageHandler, Uri> CreateHandler(Uri uri, IDockerClientConfiguration configuration, ILogger logger)
    {
        var clientOptions = new ClientOptions { Endpoint = uri, AuthProvider = new DelegateAuthProvider(configuration) };
        return CreateHandler(clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(ClientOptions clientOptions, ILogger logger)
    {
        return CreateHandler(new LegacyHttpTransportOptions(), clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(LegacyHttpTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
        var scheme = clientOptions.AuthProvider.TlsEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        var uri = new UriBuilder(clientOptions.Endpoint) { Scheme = scheme }.Uri;
        return new Tuple<HttpMessageHandler, Uri>(new ManagedHandler(logger), uri);
    }

    public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        if (content is not HttpConnectionResponseContent hijackable)
        {
            throw new NotSupportedException("The content type is not supported for stream hijacking.");
        }

        return Task.FromResult(hijackable.HijackStream());
    }
}
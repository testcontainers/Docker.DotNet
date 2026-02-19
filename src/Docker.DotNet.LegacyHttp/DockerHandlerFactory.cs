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
        var transportOptions = new LegacyHttpTransportOptions();
        Validate(transportOptions, clientOptions);
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public Tuple<HttpMessageHandler, Uri> CreateHandler(LegacyHttpTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
        Validate(transportOptions, clientOptions);

        var scheme = clientOptions.AuthProvider.TlsEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        var uri = new UriBuilder(clientOptions.Endpoint) { Scheme = scheme }.Uri;
        var handler = new ManagedHandler(logger);
        transportOptions.ConfigureHandler(handler);

        return new Tuple<HttpMessageHandler, Uri>(handler, uri);
    }

    public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        if (content is not HttpConnectionResponseContent hijackable)
        {
            throw new NotSupportedException("The content type is not supported for stream hijacking.");
        }

        return Task.FromResult(hijackable.HijackStream());
    }

    private static void Validate(LegacyHttpTransportOptions _, ClientOptions clientOptions)
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
            throw new InvalidOperationException($"The selected '{nameof(LegacyHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', or 'https', but '{scheme}' was provided.");
        }
    }
}
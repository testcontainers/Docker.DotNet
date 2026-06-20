namespace Docker.DotNet.LegacyHttp;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<LegacyHttpTransportOptions>
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<LegacyHttpTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public ResolvedTransport CreateHandler(ResolvedClientOptions clientOptions, ILogger logger)
    {
        var transportOptions = new LegacyHttpTransportOptions();
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public ResolvedTransport CreateHandler(LegacyHttpTransportOptions transportOptions, ResolvedClientOptions clientOptions, ILogger logger)
    {
        if (clientOptions is null)
        {
            throw new ArgumentNullException(nameof(clientOptions));
        }

        var endpoint = clientOptions.Endpoint;

        Validate(endpoint);

        var scheme = clientOptions.AuthProvider.TlsEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        var uri = new UriBuilder(endpoint) { Scheme = scheme }.Uri;
        var handler = new ManagedHandler(logger);
        transportOptions.ConfigureHandler(handler);

        return new ResolvedTransport(handler, uri);
    }

    public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
    {
        if (content is not HttpConnectionResponseContent hijackable)
        {
            throw new NotSupportedException("The content type is not supported for stream hijacking.");
        }

        return Task.FromResult(hijackable.HijackStream());
    }

    private static void Validate(Uri endpoint)
    {
        var scheme = endpoint.Scheme;

        if (!string.Equals(scheme, "tcp", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(LegacyHttpTransportOptions)}' can only be used with endpoint schemes 'tcp', 'http', or 'https', but '{scheme}' was provided.");
        }
    }
}
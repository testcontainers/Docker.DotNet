namespace Docker.DotNet.NPipe;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<NPipeTransportOptions>
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<NPipeTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public ResolvedTransport CreateHandler(ClientOptions clientOptions, ILogger logger)
    {
        var transportOptions = new NPipeTransportOptions();
        Validate(transportOptions, clientOptions);
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public ResolvedTransport CreateHandler(NPipeTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
    {
        Validate(transportOptions, clientOptions);

        if (clientOptions.AuthProvider.TlsEnabled)
        {
            throw new NotSupportedException("TLS is not supported over npipe.");
        }

        var uri = clientOptions.Endpoint;

        // Accept both npipe://<server>/pipe/<name> and the four-slash form
        // npipe:////<server>/pipe/<name> that docker contexts use on Windows
        // (e.g. "npipe:////./pipe/docker_engine"). The latter parses with an
        // empty Host and the server name as a path segment.
        var segments = uri.Segments;
        var pipeIndex = -1;
        for (var i = 0; i < segments.Length; i++)
        {
            if ("pipe/".Equals(segments[i], StringComparison.OrdinalIgnoreCase))
            {
                pipeIndex = i;
                break;
            }
        }

        if (pipeIndex < 0 || pipeIndex >= segments.Length - 1)
        {
            throw new InvalidOperationException("The endpoint is not a npipe URI.");
        }

        var pipeName = segments[pipeIndex + 1].TrimEnd('/');
        if (string.IsNullOrEmpty(pipeName))
        {
            throw new InvalidOperationException("The endpoint is not a npipe URI.");
        }

        var serverName = uri.Host;
        if (string.IsNullOrEmpty(serverName) && pipeIndex > 0)
        {
            serverName = segments[pipeIndex - 1].TrimEnd('/');
        }

        if (string.IsNullOrEmpty(serverName) || "localhost".Equals(serverName, StringComparison.OrdinalIgnoreCase))
        {
            serverName = ".";
        }

        uri = new UriBuilder(Uri.UriSchemeHttp, pipeName).Uri;

        var streamOpener = new ManagedHandler.StreamOpener(async (_, _, cancellationToken) =>
        {
            var clientStream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

            var dockerStream = new DockerPipeStream(clientStream);

#if NET
            var namedPipeConnectTimeout = transportOptions.ConnectTimeout;
#else
            var namedPipeConnectTimeout = (int)transportOptions.ConnectTimeout.TotalMilliseconds;
#endif

            await clientStream.ConnectAsync(namedPipeConnectTimeout, cancellationToken)
                .ConfigureAwait(false);

            return dockerStream;
        });

        var handler = new ManagedHandler(streamOpener, logger);
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

    private static void Validate(NPipeTransportOptions _, ClientOptions clientOptions)
    {
        if (clientOptions.Endpoint is null)
        {
            throw new ArgumentNullException(nameof(clientOptions), "ClientOptions.Endpoint must be set.");
        }

        var scheme = clientOptions.Endpoint.Scheme;

        if (!string.Equals(scheme, "npipe", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(NPipeTransportOptions)}' can only be used with endpoint scheme 'npipe', but '{scheme}' was provided.");
        }
    }
}
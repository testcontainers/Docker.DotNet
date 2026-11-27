namespace Docker.DotNet.NPipe;

public sealed class DockerHandlerFactory : IDockerHandlerFactory<NPipeTransportOptions>
{
    private DockerHandlerFactory()
    {
    }

    public static IDockerHandlerFactory<NPipeTransportOptions> Instance { get; }
        = new DockerHandlerFactory();

    public ResolvedTransport CreateHandler(ResolvedClientOptions clientOptions, ILogger logger)
    {
        var transportOptions = new NPipeTransportOptions();
        return CreateHandler(transportOptions, clientOptions, logger);
    }

    public ResolvedTransport CreateHandler(NPipeTransportOptions transportOptions, ResolvedClientOptions clientOptions, ILogger logger)
    {
        if (clientOptions is null)
        {
            throw new ArgumentNullException(nameof(clientOptions));
        }

        var uri = clientOptions.Endpoint;

        Validate(uri);

        if (clientOptions.AuthProvider.TlsEnabled)
        {
            throw new NotSupportedException("TLS is not supported over npipe.");
        }

        if (!TryParsePipeEndpoint(uri, out var serverName, out var pipeName))
        {
            throw new InvalidOperationException("The endpoint is not a npipe URI.");
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

    /// <summary>
    /// Reads the named pipe server and pipe name, the components of <c>\\SERVER\pipe\NAME</c>, from an <c>npipe</c> endpoint.
    /// </summary>
    /// <param name="endpoint">The endpoint to read the named pipe from.</param>
    /// <param name="serverName">The named pipe server, for example <c>.</c> in <c>\\.\pipe\docker_engine</c>.</param>
    /// <param name="pipeName">The name of the pipe to connect to.</param>
    /// <returns><see langword="true"/> when the endpoint represents a named pipe; otherwise <see langword="false"/>.</returns>
    internal static bool TryParsePipeEndpoint(Uri endpoint, out string serverName, out string pipeName)
    {
        serverName = string.Empty;
        pipeName = string.Empty;

        var segments = endpoint.Segments;

        if (segments.Length == 3 && "pipe/".Equals(segments[1], StringComparison.OrdinalIgnoreCase))
        {
            serverName = GetServerName(endpoint.Host);
            pipeName = segments[2];
            return true;
        }

        // Docker documents the named pipe endpoint in the form npipe:////SERVER/pipe/NAME, which is
        // equivalent to \\SERVER\pipe\NAME, for example npipe:////./pipe/docker_engine. System.Uri
        // parses this form with an empty host and a path that does not match the three-segment
        // layout above, so the server and pipe name are read from the endpoint's original string
        // instead: https://github.com/testcontainers/Docker.DotNet/issues/135.
        const string ServerAndPipePrefix = "npipe:////";
        const string PipeSegment = "/pipe/";

        var path = endpoint.OriginalString;

        if (!path.StartsWith(ServerAndPipePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        path = path.Substring(ServerAndPipePrefix.Length);

        var pipeSegmentIndex = path.IndexOf(PipeSegment, StringComparison.OrdinalIgnoreCase);

        if (pipeSegmentIndex < 1)
        {
            return false;
        }

        var server = path.Substring(0, pipeSegmentIndex);
        var pipe = path.Substring(pipeSegmentIndex + PipeSegment.Length);

        if (server.IndexOf('/') >= 0 || pipe.Length == 0 || pipe.IndexOf('/') >= 0)
        {
            return false;
        }

        serverName = GetServerName(server);
        pipeName = pipe;
        return true;
    }

    private static string GetServerName(string host)
    {
        return "localhost".Equals(host, StringComparison.OrdinalIgnoreCase) ? "." : host;
    }

    private static void Validate(Uri endpoint)
    {
        var scheme = endpoint.Scheme;

        if (!string.Equals(scheme, "npipe", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"The selected '{nameof(NPipeTransportOptions)}' can only be used with endpoint scheme 'npipe', but '{scheme}' was provided.");
        }
    }
}
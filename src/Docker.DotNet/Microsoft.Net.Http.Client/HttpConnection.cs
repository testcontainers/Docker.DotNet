namespace Microsoft.Net.Http.Client;

internal sealed class HttpConnection : IDisposable
{
    private static readonly ISet<string> DockerStreamHeaders = new HashSet<string>{ "application/vnd.docker.raw-stream", "application/vnd.docker.multiplexed-stream" };
    internal static readonly HttpRequestOptionsKey<bool> DisableChunkedResponseKey = new HttpRequestOptionsKey<bool>("DockerDotNetDisableChunkedResponse");

    public HttpConnection(BufferedReadStream transport)
    {
        Transport = transport;
    }

    public BufferedReadStream Transport { get; }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            // Serialize headers & send
            string rawRequest = SerializeRequest(request);
            byte[] requestBytes = Encoding.ASCII.GetBytes(rawRequest);
            await Transport.WriteAsync(requestBytes, 0, requestBytes.Length, cancellationToken);

            if (request.Content != null)
            {
                if (request.Content.Headers.ContentLength.HasValue)
                {
                    await request.Content.CopyToAsync(Transport);
                }
                else
                {
                    // The length of the data is unknown. Send it in chunked mode.
                    using (var chunkedStream = new ChunkedWriteStream(Transport))
                    {
                        await request.Content.CopyToAsync(chunkedStream);
                        await chunkedStream.EndContentAsync(cancellationToken);
                    }
                }
            }

            // Receive headers
            List<string> responseLines = await ReadResponseLinesAsync(cancellationToken);

            // Receive body and determine the response type (Content-Length, Transfer-Encoding, Opaque)
            return CreateResponseMessage(request, responseLines);
        }
        catch (Exception ex)
        {
            Dispose(); // Any errors at this layer abort the connection.
            throw new HttpRequestException("The requested failed, see inner exception for details.", ex);
        }
    }

    private string SerializeRequest(HttpRequestMessage request)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(request.Method);
        builder.Append(' ');
        builder.Append(request.GetAddressLineProperty());
        builder.Append(" HTTP/");
        builder.Append(request.Version.ToString(2));
        builder.Append("\r\n");

        builder.Append(request.Headers);

        if (request.Content != null)
        {
            // Force the content to compute its content length if it has not already.
            var contentLength = request.Content.Headers.ContentLength;
            if (contentLength.HasValue)
            {
                request.Content.Headers.ContentLength = contentLength.Value;
            }

            builder.Append(request.Content.Headers);
            if (!contentLength.HasValue)
            {
                // Add header for chunked mode.
                builder.Append("Transfer-Encoding: chunked\r\n");
            }
        }
        // Headers end with an empty line
        builder.Append("\r\n");
        return builder.ToString();
    }

    private async Task<List<string>> ReadResponseLinesAsync(CancellationToken cancellationToken)
    {
        var lines = new List<string>(12);

        do
        {
            var line = await Transport.ReadLineAsync(cancellationToken)
                .ConfigureAwait(false);

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            lines.Add(line);
        }
        while (true);

        return lines;
    }

    private HttpResponseMessage CreateResponseMessage(HttpRequestMessage request, List<string> responseLines)
    {
        string responseLine = responseLines.First();
        // HTTP/1.1 200 OK
        string[] responseLineParts = responseLine.Split(new[] { ' ' }, 3);
        // TODO: Verify HTTP/1.0 or 1.1.
        if (responseLineParts.Length < 2)
        {
            throw new HttpRequestException("Invalid response line: " + responseLine);
        }

        if (int.TryParse(responseLineParts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var statusCode))
        {
            // TODO: Validate range
        }
        else
        {
            throw new HttpRequestException("Invalid status code: " + responseLineParts[1]);
        }
        HttpResponseMessage response = new HttpResponseMessage((HttpStatusCode)statusCode);
        if (responseLineParts.Length >= 3)
        {
            response.ReasonPhrase = responseLineParts[2];
        }
        var content = new HttpConnectionResponseContent(this);
        response.Content = content;

        foreach (var rawHeader in responseLines.Skip(1))
        {
            int colonOffset = rawHeader.IndexOf(':');
            if (colonOffset <= 0)
            {
                throw new HttpRequestException("The given header line format is invalid: " + rawHeader);
            }
            string headerName = rawHeader.Substring(0, colonOffset);
            string headerValue = rawHeader.Substring(colonOffset + 2);
            if (!response.Headers.TryAddWithoutValidation(headerName, headerValue))
            {
                bool success = response.Content.Headers.TryAddWithoutValidation(headerName, headerValue);
                System.Diagnostics.Debug.Assert(success, "Failed to add response header: " + rawHeader);
            }
        }

        // Response handling is based on headers and type. The implementation currently covers
        // four main cases:
        //
        // 1. Chunked transfer encoding (HTTP/1.1 `Transfer-Encoding: chunked`)
        // 2. HTTP responses with a `Content-Length` header
        //     - For 101 Switching Protocols, `Content-Length` is ignored per RFC 9110
        // 3. Protocol upgrades (HTTP 101 + `Upgrade: tcp`)
        //     - e.g., `/containers/{id}/attach` or `/exec/{id}/start`
        // 4. Streaming responses without connection upgrade headers
        //     - e.g., `/containers/{id}/logs`
        //
        // This separation ensures chunked framing, streaming, and upgraded connections are handled
        // correctly, while tolerating proxies that incorrectly send `Content-Length: 0` on upgrades.

        var isSwitchingProtocols = response.StatusCode == HttpStatusCode.SwitchingProtocols;

        var isConnectionUpgrade = response.Headers.TryGetValues("Upgrade", out var responseHeaderValues)
            && responseHeaderValues.Any(header => "tcp".Equals(header, StringComparison.OrdinalIgnoreCase));

        var isStream = content.Headers.TryGetValues("Content-Type", out var contentHeaderValues)
            && contentHeaderValues.Any(DockerStreamHeaders.Contains);

        // Treat the response as chunked for standard HTTP chunked or Docker raw-streams,
        // but not for upgraded connections.
        var disableChunkedResponse = request.Options.TryGetValue(DisableChunkedResponseKey, out var disableChunked) && disableChunked;
        var isChunkedTransferEncoding = !disableChunkedResponse &&
                                        ((response.Headers.TransferEncodingChunked.GetValueOrDefault() && !isConnectionUpgrade)
                                         || (!isConnectionUpgrade && isStream));

        if (isSwitchingProtocols && isConnectionUpgrade)
        {
            content.ResolveResponseStream(false, true);
        }
        else
        {
            content.ResolveResponseStream(isChunkedTransferEncoding, isConnectionUpgrade);
        }

        return response;
    }

    public void Dispose()
    {
        Transport.Dispose();
    }
}
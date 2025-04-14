namespace Microsoft.Net.Http.Client;

internal sealed class HttpConnection : IDisposable
{
    private static readonly ISet<string> DockerStreamHeaders = new HashSet<string>{ "application/vnd.docker.raw-stream", "application/vnd.docker.multiplexed-stream" };

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
            return CreateResponseMessage(responseLines);
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

    private HttpResponseMessage CreateResponseMessage(List<string> responseLines)
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

        var isStream = content.Headers.TryGetValues("Content-Type", out var headerValues)
            && headerValues.Any(header => DockerStreamHeaders.Contains(header));

        content.ResolveResponseStream(chunked: response.Headers.TransferEncodingChunked.HasValue && response.Headers.TransferEncodingChunked.Value && !isStream);
        return response;
    }

    public void Dispose()
    {
        Transport.Dispose();
    }
}
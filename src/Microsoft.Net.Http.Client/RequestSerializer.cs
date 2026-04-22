namespace Microsoft.Net.Http.Client;

internal abstract class RequestSerializer<T>
{
    public abstract ReadOnlyMemory<byte> SerializeRequest(HttpRequestMessage request);

    protected void SerializeRequest(T buffer, HttpRequestMessage request)
    {
        WriteString(buffer, request.Method.Method);
        WriteBytes(buffer, " "u8);
        WriteString(buffer, request.GetAddressLineProperty());
        WriteBytes(buffer, " HTTP/"u8);
        WriteString(buffer, request.Version.ToString(2));
        WriteBytes(buffer, "\r\n"u8);

        AppendHeaders(buffer, request.Headers);

        if (request.Content != null)
        {
            // Force the content to compute its content length if it has not already.
            var contentLength = request.Content.Headers.ContentLength;
            if (contentLength.HasValue)
            {
                request.Content.Headers.ContentLength = contentLength.Value;
            }

            AppendHeaders(buffer, request.Content.Headers);
            if (!contentLength.HasValue)
            {
                // Add header for chunked mode.
                WriteBytes(buffer, "Transfer-Encoding: chunked\r\n"u8);
            }
        }
        // Headers end with an empty line
        WriteBytes(buffer, "\r\n"u8);
    }

    protected void AppendHeaders(T buffer, HttpHeaders headers)
    {
        foreach (var header in headers)
        {
            WriteString(buffer, header.Key);
            WriteBytes(buffer, ": "u8);
            var first = false;
            foreach (var value in header.Value)
            {
                if (first)
                {
                    WriteBytes(buffer, ", "u8);
                }
                first = true;

                WriteString(buffer, value);
            }
            WriteBytes(buffer, "\r\n"u8);
        }
    }

    protected abstract void WriteString(T buffer, string? str);
    protected abstract void WriteBytes(T buffer, ReadOnlySpan<byte> span);
}

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
internal sealed class RequestSerializerArrayBufferWriter : RequestSerializer<ArrayBufferWriter<byte>>
{
    public override ReadOnlyMemory<byte> SerializeRequest(HttpRequestMessage request)
    {
        var buffer = new ArrayBufferWriter<byte>();
        SerializeRequest(buffer, request);
        return buffer.WrittenMemory;
    }

    protected override void WriteString(ArrayBufferWriter<byte> buffer, string? str)
    {
        if (str is null) return;
#if NET
        Encoding.ASCII.GetBytes(str, buffer);
#else
        var length = Encoding.ASCII.GetByteCount(str);
        var span = buffer.GetSpan(length);
        var written = Encoding.ASCII.GetBytes(str, span);
        buffer.Advance(written);
#endif
    }

    protected override void WriteBytes(ArrayBufferWriter<byte> buffer, ReadOnlySpan<byte> span)
    {
        buffer.Write(span);
    }
}
#else
internal sealed class RequestSerializerMemoryStream : RequestSerializer<MemoryStream>
{
    public override ReadOnlyMemory<byte> SerializeRequest(HttpRequestMessage request)
    {
        using var buffer = new MemoryStream();
        SerializeRequest(buffer, request);
        if (buffer.TryGetBuffer(out var segment))
        {
            return segment;
        }
        return buffer.ToArray();
    }

    protected override void WriteString(MemoryStream buffer, string? str)
    {
        if (str is null) return;
        var bytes = Encoding.ASCII.GetBytes(str);
        buffer.Write(bytes, 0, bytes.Length);
    }

    protected override void WriteBytes(MemoryStream buffer, ReadOnlySpan<byte> span)
    {
        var bytes = span.ToArray();
        buffer.Write(bytes, 0, bytes.Length);
    }
}
#endif

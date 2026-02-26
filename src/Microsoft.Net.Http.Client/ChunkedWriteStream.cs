namespace Microsoft.Net.Http.Client;

internal sealed class ChunkedWriteStream : Stream
{
    private static readonly byte[] EndOfContentBytes = Encoding.ASCII.GetBytes("0\r\n\r\n");

    private readonly Stream _inner;

    public ChunkedWriteStream(Stream stream)
    {
        _inner = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public override bool CanRead
        => false;

    public override bool CanSeek
        => false;

    public override bool CanWrite
        => true;

    public override long Length
        => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush()
        => _inner.Flush();

    public override Task FlushAsync(CancellationToken cancellationToken)
        => _inner.FlushAsync(cancellationToken);

    public override int Read(byte[] buffer, int offset, int count)
        => throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin)
        => throw new NotSupportedException();

    public override void SetLength(long value)
        => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
        => throw new NotSupportedException();

    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (count == 0)
        {
            return;
        }

        const string crlf = "\r\n";

        var chunkHeader = count.ToString("X") + crlf;
        var headerBytes = Encoding.ASCII.GetBytes(chunkHeader);

        // Write the chunk header
        await _inner.WriteAsync(headerBytes, 0, headerBytes.Length, cancellationToken)
            .ConfigureAwait(false);

        // Write the chunk data
        await _inner.WriteAsync(buffer, offset, count, cancellationToken)
            .ConfigureAwait(false);

        // Write the chunk footer (CRLF)
        await _inner.WriteAsync(headerBytes, headerBytes.Length - 2, 2, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task EndContentAsync(CancellationToken cancellationToken)
        => _inner.WriteAsync(EndOfContentBytes, 0, EndOfContentBytes.Length, cancellationToken);
}
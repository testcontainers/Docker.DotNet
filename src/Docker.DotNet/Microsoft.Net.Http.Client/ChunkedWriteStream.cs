namespace Microsoft.Net.Http.Client;

internal sealed class ChunkedWriteStream : Stream
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    , IAsyncDisposable
#endif
{
    private static readonly byte[] EndOfContentBytes = Encoding.ASCII.GetBytes("0\r\n\r\n");

    private readonly Stream _inner;
    private bool _disposed;

    public ChunkedWriteStream(Stream stream)
    {
        _inner = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length
    {
        get { throw new NotImplementedException(); }
    }

    public override long Position
    {
        get { throw new NotImplementedException(); }
        set { throw new NotImplementedException(); }
    }

    public override void Flush()
    {
        _inner.Flush();
    }

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return _inner.FlushAsync(cancellationToken);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

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
    {
        return _inner.WriteAsync(EndOfContentBytes, 0, EndOfContentBytes.Length, cancellationToken);
    }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (buffer.Length == 0)
        {
            return;
        }

        const string crlf = "\r\n";

        var chunkHeader = buffer.Length.ToString("X") + crlf;
        var headerBytes = Encoding.ASCII.GetBytes(chunkHeader);

        // Write the chunk header
        await _inner.WriteAsync(headerBytes.AsMemory(), cancellationToken)
            .ConfigureAwait(false);

        // Write the chunk data
        await _inner.WriteAsync(buffer, cancellationToken)
            .ConfigureAwait(false);

        // Write the chunk footer (CRLF)
        await _inner.WriteAsync(headerBytes.AsMemory(headerBytes.Length - 2, 2), cancellationToken)
            .ConfigureAwait(false);
    }
#endif

    protected override void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _disposed = true;
            // Note: We don't dispose _inner here as it's owned by the caller
        }
        base.Dispose(disposing);
    }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    public new ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            _disposed = true;
            // Note: We don't dispose _inner here as it's owned by the caller
        }
        GC.SuppressFinalize(this);
        return default;
    }
#endif
}
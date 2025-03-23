namespace Microsoft.Net.Http.Client;

internal sealed class ChunkedWriteStream : Stream
{
    private static readonly byte[] s_EndContentBytes = Encoding.ASCII.GetBytes("0\r\n\r\n");

    private readonly Stream _inner;

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

    protected override void Dispose(bool disposing)
    {
        // base.Dispose(disposing);

        if (disposing)
        {
            // _inner.Dispose();
        }
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

        var chunkSize = Encoding.ASCII.GetBytes(count.ToString("x") + "\r\n");
        await _inner.WriteAsync(chunkSize, 0, chunkSize.Length, cancellationToken).ConfigureAwait(false);
        await _inner.WriteAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
        await _inner.WriteAsync(chunkSize, chunkSize.Length - 2, 2, cancellationToken).ConfigureAwait(false);
    }

    public Task EndContentAsync(CancellationToken cancellationToken)
    {
        return _inner.WriteAsync(s_EndContentBytes, 0, s_EndContentBytes.Length, cancellationToken);
    }
}
namespace Microsoft.Net.Http.Client;

internal sealed class ChunkedReadStream : WriteClosableStream
{
    private readonly BufferedReadStream _inner;
    private int _chunkBytesRemaining;
    private bool _done;

    public ChunkedReadStream(BufferedReadStream stream)
    {
        _inner = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public override bool CanRead
    {
        get { return _inner.CanRead; }
    }

    public override bool CanSeek
    {
        get { return false; }
    }

    public override bool CanTimeout
    {
        get { return _inner.CanTimeout; }
    }

    public override bool CanWrite
    {
        get { return false; }
    }

    public override bool CanCloseWrite
    {
        get { return _inner.CanCloseWrite; }
    }

    public override long Length
    {
        get { throw new NotSupportedException(); }
    }

    public override long Position
    {
        get { throw new NotSupportedException(); }
        set { throw new NotSupportedException(); }
    }

    public override int ReadTimeout
    {
        get
        {
            return _inner.ReadTimeout;
        }
        set
        {
            _inner.ReadTimeout = value;
        }
    }

    public override int WriteTimeout
    {
        get
        {
            return _inner.WriteTimeout;
        }
        set
        {
            _inner.WriteTimeout = value;
        }
    }

    protected override void Dispose(bool disposing)
    {
        // base.Dispose(disposing);

        if (disposing)
        {
            // _inner.Dispose();
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (_done)
        {
            return 0;
        }

        if (_chunkBytesRemaining == 0)
        {
            var headerLine = await _inner.ReadLineAsync(cancellationToken)
                .ConfigureAwait(false);

            if (!int.TryParse(headerLine, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _chunkBytesRemaining))
            {
                throw new IOException($"Invalid chunk header encountered: '{headerLine}'.");
            }
        }

        var readBytesCount = 0;

        if (_chunkBytesRemaining > 0)
        {
            var remainingBytesCount = Math.Min(_chunkBytesRemaining, count);

            readBytesCount = await _inner.ReadAsync(buffer, offset, remainingBytesCount, cancellationToken)
                .ConfigureAwait(false);

            if (readBytesCount == 0)
            {
                throw new EndOfStreamException();
            }

            _chunkBytesRemaining -= readBytesCount;
        }

        if (_chunkBytesRemaining == 0)
        {
            var emptyLine = await _inner.ReadLineAsync(cancellationToken)
                .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(emptyLine))
            {
                throw new IOException($"Expected an empty line, but received: '{emptyLine}'.");
            }

            _done = readBytesCount == 0;
        }

        return readBytesCount;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _inner.Write(buffer, offset, count);
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return _inner.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Flush()
    {
        _inner.Flush();
    }

    public override void CloseWrite()
    {
        _inner.CloseWrite();
    }
}
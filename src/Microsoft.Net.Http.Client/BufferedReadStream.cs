namespace Microsoft.Net.Http.Client;

internal sealed class BufferedReadStream : WriteClosableStream, IPeekableStream
{
    private readonly Stream _inner;

    private readonly Socket _socket;

    private readonly byte[] _buffer;

    private readonly ILogger _logger;

    private int _bufferRefCount;

    private int _bufferOffset;

    private int _bufferCount;

    public BufferedReadStream(Stream inner, Socket socket, ILogger logger)
        : this(inner, socket, 8192, logger)
    {
    }

    public BufferedReadStream(Stream inner, Socket socket, int bufferLength, ILogger logger)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        _socket = socket;
        _buffer = ArrayPool<byte>.Shared.Rent(bufferLength);
        _logger = logger;
        _bufferRefCount = 1;
    }

    public override bool CanRead
        => _inner.CanRead || _bufferCount > 0;

    public override bool CanSeek
        => false;

    public override bool CanWrite
        => _inner.CanWrite;

    public override bool CanTimeout
        => _inner.CanTimeout;

    public override bool CanCloseWrite
        => _socket != null || _inner is WriteClosableStream;

    public override long Length
        => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Interlocked.Exchange(ref _bufferRefCount, 0) == 1)
            {
                ArrayPool<byte>.Shared.Return(_buffer);
            }

            _inner.Dispose();
        }

        base.Dispose(disposing);
    }

    public override void Flush()
        => _inner.Flush();

    public override Task FlushAsync(CancellationToken cancellationToken)
        => _inner.FlushAsync(cancellationToken);

    public override int Read(byte[] buffer, int offset, int count)
    {
        int read = ReadBuffer(buffer, offset, count);
        if (read > 0)
        {
            return read;
        }

        return _inner.Read(buffer, offset, count);
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        int read = ReadBuffer(buffer, offset, count);
        if (read > 0)
        {
            return Task.FromResult(read);
        }

        return _inner.ReadAsync(buffer, offset, count, cancellationToken);
    }

    public override long Seek(long offset, SeekOrigin origin)
        => throw new NotSupportedException();

    public override void SetLength(long value)
        => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
        => _inner.Write(buffer, offset, count);

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => _inner.WriteAsync(buffer, offset, count, cancellationToken);

    public override void CloseWrite()
    {
        if (_socket != null)
        {
            _socket.Shutdown(SocketShutdown.Send);
            return;
        }

        if (_inner is WriteClosableStream writeClosableStream)
        {
            writeClosableStream.CloseWrite();
            return;
        }

        throw new NotSupportedException("Cannot shutdown write on this transport");
    }

    public bool Peek(byte[] buffer, uint toPeek, out uint peeked, out uint available, out uint remaining)
    {
        int read = PeekBuffer(buffer, toPeek, out peeked, out available, out remaining);
        if (read > 0)
        {
            return true;
        }

        if (_inner is IPeekableStream peekableStream)
        {
            return peekableStream.Peek(buffer, toPeek, out peeked, out available, out remaining);
        }

        throw new NotSupportedException("_inner stream isn't a peekable stream");
    }

    public async Task<string> ReadLineAsync(CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();

        const byte cr = (byte)'\r';
        const byte lf = (byte)'\n';

        bool crFound = false;

        while (true)
        {
            if (_bufferCount == 0)
            {
                _bufferOffset = 0;

                _bufferCount = await _inner.ReadAsync(_buffer, 0, _buffer.Length, cancellationToken)
                    .ConfigureAwait(false);

                if (_bufferCount == 0)
                {
                    return null;
                }
            }

            if (crFound)
            {
                if (_buffer[_bufferOffset] == lf)
                {
                    _bufferOffset += 1;
                    _bufferCount -= 1;
                    break;
                }
                crFound = false;
                memoryStream.WriteByte(cr);
            }

            var crIndex = _buffer.AsSpan(_bufferOffset, _bufferCount).IndexOf(cr);
            if (crIndex != -1)
            {
                memoryStream.Write(_buffer, _bufferOffset, crIndex);
                _bufferOffset += crIndex + 1;
                _bufferCount -= crIndex + 1;

                if (_bufferCount > 0)
                {
                    if (_buffer[_bufferOffset] == lf)
                    {
                        _bufferOffset++;
                        _bufferCount--;
                        break;
                    }
                    memoryStream.WriteByte(cr);
                }
                else
                {
                    crFound = true;
                }
            }
            else
            {
                memoryStream.Write(_buffer, _bufferOffset, _bufferCount);
                _bufferCount = 0;
            }
        }

        return Encoding.ASCII.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
    }

    private int ReadBuffer(byte[] buffer, int offset, int count)
    {
        if (_bufferCount > 0)
        {
            int toCopy = Math.Min(_bufferCount, count);
            Buffer.BlockCopy(_buffer, _bufferOffset, buffer, offset, toCopy);
            _bufferOffset += toCopy;
            _bufferCount -= toCopy;
            return toCopy;
        }

        return 0;
    }

    private int PeekBuffer(byte[] buffer, uint toPeek, out uint peeked, out uint available, out uint remaining)
    {
        if (_bufferCount > 0)
        {
            int toCopy = Math.Min(_bufferCount, (int)toPeek);
            Buffer.BlockCopy(_buffer, _bufferOffset, buffer, 0, toCopy);
            peeked = (uint)toCopy;
            available = (uint)_bufferCount;
            remaining = available - peeked;
            return toCopy;
        }

        peeked = 0;
        available = 0;
        remaining = 0;
        return 0;
    }
}
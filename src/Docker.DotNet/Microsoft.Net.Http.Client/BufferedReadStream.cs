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
    {
        get { return _inner.CanRead || _bufferCount > 0; }
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
        get { return _inner.CanWrite; }
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

    public override bool CanCloseWrite => _socket != null || _inner is WriteClosableStream;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (Interlocked.Decrement(ref _bufferRefCount) == 0)
            {
                ArrayPool<byte>.Shared.Return(_buffer);
            }

            _inner.Dispose();
        }

        base.Dispose(disposing);
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

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return _inner.FlushAsync(cancellationToken);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        _inner.Write(buffer, offset, count);
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return _inner.WriteAsync(buffer, offset, count, cancellationToken);
    }

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
        const char nullChar = '\0';

        const char cr = '\r';

        const char lf = '\n';

        if (_bufferCount == 0)
        {
            _bufferOffset = 0;

            var bufferNotInUse = Interlocked.Increment(ref _bufferRefCount) > 1;

            try
            {
                if (bufferNotInUse)
                {
                    _bufferCount = await _inner.ReadAsync(_buffer, 0, _buffer.Length, cancellationToken)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Failed to read from buffer.");
                throw;
            }
            finally
            {
                var bufferReleased = Interlocked.Decrement(ref _bufferRefCount) == 0;

                if (bufferNotInUse && bufferReleased)
                {
                    ArrayPool<byte>.Shared.Return(_buffer);
                }
            }
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            var content = Encoding.ASCII.GetString(_buffer.TakeWhile(value => value != nullChar).ToArray());
            content = content.Replace("\r", "<CR>");
            content = content.Replace("\n", "<LF>");
            _logger.LogDebug("Raw buffer content: '{Content}'.", content);
        }

        var start = _bufferOffset;

        var end = -1;

        for (var i = _bufferOffset; i < _buffer.Length; i++)
        {
            // If a null terminator is found, skip the rest of the buffer.
            if (_buffer[i] == nullChar)
            {
                _logger.LogDebug("Null terminator found at position: {Position}.", i);
                end = i;
                break;
            }

            // Check if current byte is CR and the next byte is LF.
            if (_buffer[i] == cr && i + 1 < _buffer.Length && _buffer[i + 1] == lf)
            {
                _logger.LogDebug("CRLF found at positions {CR} and {LF}.", i, i + 1);
                end = i;
                break;
            }
        }

        // No CRLF found, process the entire remaining buffer.
        if (end == -1)
        {
            end = _buffer.Length;
            _logger.LogDebug("No CRLF found. Setting end position to buffer length: {End}.", end);
        }
        else
        {
            _bufferCount -= end - start + 2;
            _bufferOffset = end + 2;
            _logger.LogDebug("CRLF found. Consumed {Consumed} bytes. New offset: {Offset}, Remaining count: {RemainingBytes}.", end - start + 2, _bufferOffset, _bufferCount);
        }

        var length = end - start;
        var line = Encoding.ASCII.GetString(_buffer, start, length);

        _logger.LogDebug("String from positions {Start} to {End} (length {Length}): '{Line}'.", start, end, length, line);
        return line;
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
            peeked = (uint) toCopy;
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
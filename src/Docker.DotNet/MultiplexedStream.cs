namespace Docker.DotNet;

public sealed class MultiplexedStream : IDisposable, IPeekableStream
{
    private const int BufferSize = 16384;
    private readonly Stream _stream;
    private TargetStream _target;
    private int _remaining;
    private readonly byte[] _header = new byte[8];
    private readonly bool _multiplexed;

    public MultiplexedStream(Stream stream, bool multiplexed)
    {
        _stream = stream;
        _multiplexed = multiplexed;
    }

    public void Dispose()
    {
        _stream.Dispose();
    }

    public enum TargetStream : byte
    {
        StandardIn = 0,
        StandardOut = 1,
        StandardError = 2
    }

    public readonly struct ReadResult
    {
        public ReadResult(TargetStream target, int count)
        {
            Target = target;
            Count = count;
        }

        public TargetStream Target { get; }

        public int Count { get; }

        public bool EOF => Count == 0;
    }

    public void CloseWrite()
    {
        if (_stream is WriteClosableStream writeClosableStream)
        {
            writeClosableStream.CloseWrite();
        }
    }

    public bool Peek(byte[] buffer, uint toPeek, out uint peeked, out uint available, out uint remaining)
    {
        if (_stream is IPeekableStream peekableStream)
        {
            return peekableStream.Peek(buffer, toPeek, out peeked, out available, out remaining);
        }

        throw new NotSupportedException("_stream isn't a peekable stream");
    }

    public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        return _stream.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public async Task<ReadResult> ReadOutputAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        int readBytesCount;

        if (!_multiplexed)
        {
            readBytesCount = await _stream.ReadAsync(buffer, offset, count, cancellationToken)
                .ConfigureAwait(false);

            return new ReadResult(TargetStream.StandardOut, readBytesCount);
        }

        while (_remaining == 0)
        {
            for (var i = 0; i < _header.Length;)
            {
                readBytesCount = await _stream.ReadAsync(_header, i, _header.Length - i, cancellationToken)
                    .ConfigureAwait(false);

                if (readBytesCount == 0)
                {
                    if (i == 0)
                    {
                        return new ReadResult();
                    }

                    throw new EndOfStreamException();
                }

                i += readBytesCount;
            }

            if (Enum.IsDefined(typeof(TargetStream), _header[0]))
            {
                _target = (TargetStream)_header[0];
            }
            else
            {
                throw new IOException($"Unknown stream type: '{_header[0]}'.");
            }

            _remaining = (_header[4] << 24) |
                         (_header[5] << 16) |
                         (_header[6] << 8) |
                         _header[7];
        }

        var remainingBytesCount = Math.Min(count, _remaining);

        readBytesCount = await _stream.ReadAsync(buffer, offset, remainingBytesCount, cancellationToken)
            .ConfigureAwait(false);

        if (readBytesCount == 0)
        {
            throw new EndOfStreamException();
        }

        _remaining -= readBytesCount;
        return new ReadResult(_target, readBytesCount);
    }

    public async Task<(string stdout, string stderr)> ReadOutputToEndAsync(CancellationToken cancellationToken)
    {
        using MemoryStream stdoutMemoryStream = new MemoryStream(), stderrMemoryStream = new MemoryStream();

        using StreamReader stdoutStreamReader = new StreamReader(stdoutMemoryStream), stderrStreamReader = new StreamReader(stderrMemoryStream);

        await CopyOutputToAsync(Stream.Null, stdoutMemoryStream, stderrMemoryStream, cancellationToken)
            .ConfigureAwait(false);

        stdoutMemoryStream.Seek(0, SeekOrigin.Begin);
        stderrMemoryStream.Seek(0, SeekOrigin.Begin);

        var stdoutReadTask = stdoutStreamReader.ReadToEndAsync();
        var stderrReadTask = stderrStreamReader.ReadToEndAsync();
        await Task.WhenAll(stdoutReadTask, stderrReadTask)
            .ConfigureAwait(false);

        return (stdoutReadTask.Result, stderrReadTask.Result);
    }

    public async Task CopyFromAsync(Stream input, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);

        try
        {
            for (;;)
            {
                var readBytesCount = await input.ReadAsync(buffer, 0, buffer.Length, cancellationToken)
                    .ConfigureAwait(false);

                if (readBytesCount == 0)
                {
                    break;
                }

                await WriteAsync(buffer, 0, readBytesCount, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public async Task CopyOutputToAsync(Stream stdin, Stream stdout, Stream stderr, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);

        try
        {
            for (;;)
            {
                var result = await ReadOutputAsync(buffer, 0, buffer.Length, cancellationToken)
                    .ConfigureAwait(false);

                if (result.EOF)
                {
                    return;
                }

                Stream stream;

                switch (result.Target)
                {
                    case TargetStream.StandardIn:
                        stream = stdin;
                        break;
                    case TargetStream.StandardOut:
                        stream = stdout;
                        break;
                    case TargetStream.StandardError:
                        stream = stderr;
                        break;
                    default:
                        throw new IOException($"Unknown stream type: '{result.Target}'.");
                }

                await stream.WriteAsync(buffer, 0, result.Count, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
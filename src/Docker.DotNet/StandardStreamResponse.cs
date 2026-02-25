namespace Docker.DotNet;

internal sealed class StandardStreamResponse : Stream
{
    private readonly HttpResponseMessage _response;

    private readonly Stream _stream;

    public StandardStreamResponse(HttpResponseMessage response, Stream stream)
    {
        _response = response ?? throw new ArgumentNullException(nameof(response));
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public HttpHeaders Headers
        => _response.Headers;

    public override bool CanRead
        => _stream.CanRead;

    public override bool CanSeek
        => _stream.CanSeek;

    public override bool CanWrite
        => _stream.CanWrite;

    public override long Length
        => _stream.Length;

    public override long Position
    {
        get => _stream.Position;
        set => _stream.Position = value;
    }

    public override void Flush()
        => _stream.Flush();

    public override int Read(byte[] buffer, int offset, int count)
        => _stream.Read(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin)
        => _stream.Seek(offset, origin);

    public override void SetLength(long value)
        => _stream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count)
        => _stream.Write(buffer, offset, count);

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => _stream.ReadAsync(buffer, offset, count, cancellationToken);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        => _stream.ReadAsync(buffer, cancellationToken);
#endif

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => _stream.WriteAsync(buffer, offset, count, cancellationToken);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        => _stream.WriteAsync(buffer, cancellationToken);
#endif

    public override Task FlushAsync(CancellationToken cancellationToken)
        => _stream.FlushAsync(cancellationToken);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _stream.Dispose();
            _response.Dispose();
        }

        base.Dispose(disposing);
    }
}
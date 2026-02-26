namespace Docker.DotNet.NativeHttp;

internal sealed class WriteClosableStreamWrapper(Stream stream) : WriteClosableStream
{
    private readonly Stream _stream = stream ?? throw new ArgumentNullException(nameof(stream));

    public override bool CanRead
        => _stream.CanRead;

    public override bool CanSeek
        => _stream.CanSeek;

    public override bool CanWrite
        => _stream.CanWrite;

    public override bool CanCloseWrite
        => true;

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

    public override void CloseWrite()
        => _stream.Close();

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _stream.Dispose();
        }

        base.Dispose(disposing);
    }
}
using System;
using System.IO;
using Microsoft.Net.Http.Client;

namespace Microsoft.Net.Http.Client;


public class WriteClosableStreamWrapper : WriteClosableStream
{
    private readonly Stream _baseStream;

    public WriteClosableStreamWrapper(Stream baseStream)
    {
        _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
    }

    public override void CloseWrite()
    {
        _baseStream.Close(); // Replace with half-close logic if available
    }

    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => _baseStream.CanSeek;
    public override bool CanWrite => _baseStream.CanWrite;
    public override bool CanCloseWrite => true;
    public override long Length => _baseStream.Length;

    public override long Position
    {
        get => _baseStream.Position;
        set => _baseStream.Position = value;
    }

    public override void Flush() => _baseStream.Flush();

    public override int Read(byte[] buffer, int offset, int count) =>
        _baseStream.Read(buffer, offset, count);

    public override long Seek(long offset, SeekOrigin origin) =>
        _baseStream.Seek(offset, origin);

    public override void SetLength(long value) =>
        _baseStream.SetLength(value);

    public override void Write(byte[] buffer, int offset, int count) =>
        _baseStream.Write(buffer, offset, count);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _baseStream.Dispose();
        }
        base.Dispose(disposing);
    }
}
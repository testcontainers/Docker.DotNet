namespace Docker.DotNet.NPipe;

internal sealed class DockerPipeStream : WriteClosableStream, IPeekableStream
{
    private readonly EventWaitHandle _event = new EventWaitHandle(false, EventResetMode.AutoReset);

    private readonly PipeStream _stream;

    public DockerPipeStream(PipeStream stream)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new PlatformNotSupportedException("DockerPipeStream is only supported on Windows.");
        }

        _stream = stream;
    }

    public override bool CanRead
        => true;

    public override bool CanSeek
        => false;

    public override bool CanWrite
        => true;

    public override bool CanCloseWrite
        => true;

    public override long Length
        => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    [DllImport("api-ms-win-core-file-l1-1-0.dll", SetLastError = true)]
    private static extern int WriteFile(SafeHandle handle, IntPtr buffer, int numBytesToWrite, IntPtr numBytesWritten, ref NativeOverlapped overlapped);

    [DllImport("api-ms-win-core-io-l1-1-0.dll", SetLastError = true)]
    private static extern int GetOverlappedResult(SafeHandle handle, ref NativeOverlapped overlapped, out int numBytesWritten, int wait);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool PeekNamedPipe(SafeHandle handle, byte[] buffer, uint nBufferSize, ref uint bytesRead, ref uint bytesAvail, ref uint bytesLeftThisMessage);

    public override void Flush()
        => throw new NotSupportedException();

    public override int Read(byte[] buffer, int offset, int count)
        => _stream.Read(buffer, offset, count);

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => _stream.ReadAsync(buffer, offset, count, cancellationToken);

    public bool Peek(byte[] buffer, uint toPeek, out uint peeked, out uint available, out uint remaining)
    {
        peeked = 0;
        available = 0;
        remaining = 0;

        bool aPeekedSuccess = PeekNamedPipe(
            _stream.SafePipeHandle,
            buffer, toPeek,
            ref peeked, ref available, ref remaining);

        var error = Marshal.GetLastWin32Error();

        if (error == 0 && aPeekedSuccess)
        {
            return true;
        }

        return false;
    }

    public override long Seek(long offset, SeekOrigin origin)
        => throw new NotSupportedException();

    public override void SetLength(long value)
        => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
        => _stream.Write(buffer, offset, count);

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => _stream.WriteAsync(buffer, offset, count, cancellationToken);

    public override void CloseWrite()
    {
        const int errorIoPending = 997;

#pragma warning disable CA1416
        // The Docker daemon expects a write of zero bytes to signal the end of writes. Use native
        // calls to achieve this since CoreCLR ignores a zero-byte write.
        var overlapped = new NativeOverlapped();

        var handle = _event.GetSafeWaitHandle();

        // Set the low bit to tell Windows not to send the result of this IO to the
        // completion port.
        overlapped.EventHandle = (IntPtr)(handle.DangerousGetHandle().ToInt64() | 1);
#pragma warning restore CA1416

        if (WriteFile(_stream.SafePipeHandle, IntPtr.Zero, 0, IntPtr.Zero, ref overlapped) == 0)
        {
            if (Marshal.GetLastWin32Error() == errorIoPending)
            {
                if (GetOverlappedResult(_stream.SafePipeHandle, ref overlapped, out _, 1) == 0)
                {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }
            else
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _event.Dispose();
            _stream.Dispose();
        }
    }
}
namespace Docker.DotNet;

using System.Net.WebSockets;

internal sealed class PortainerWebSocketStream : WriteClosableStream
{
    private readonly ClientWebSocket _webSocket;
    private readonly SemaphoreSlim _writeLock = new(1, 1);
    private readonly byte[] _receiveBuffer = new byte[8192];
    private byte[] _pendingBuffer = Array.Empty<byte>();
    private int _pendingOffset;
    private bool _disposed;

    public PortainerWebSocketStream(ClientWebSocket webSocket)
    {
        _webSocket = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
    }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override bool CanCloseWrite => true;

    public override long Length => throw new NotSupportedException();

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return ReadAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
    }

    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(PortainerWebSocketStream));
        }

        if (_pendingOffset >= _pendingBuffer.Length)
        {
            _pendingBuffer = await ReceiveMessageAsync(cancellationToken).ConfigureAwait(false);
            _pendingOffset = 0;
        }

        if (_pendingBuffer.Length == 0)
        {
            return 0;
        }

        var remaining = _pendingBuffer.Length - _pendingOffset;
        var toCopy = Math.Min(count, remaining);
        Array.Copy(_pendingBuffer, _pendingOffset, buffer, offset, toCopy);
        _pendingOffset += toCopy;
        return toCopy;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
    }

    public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(PortainerWebSocketStream));
        }

        await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer, offset, count), WebSocketMessageType.Binary, true, cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            _writeLock.Release();
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void CloseWrite()
    {
        if (_disposed)
        {
            return;
        }

        if (_webSocket.State == WebSocketState.Open || _webSocket.State == WebSocketState.CloseReceived)
        {
            _webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None)
                .GetAwaiter()
                .GetResult();
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            base.Dispose(disposing);
            return;
        }

        _disposed = true;

        if (disposing)
        {
            if (_webSocket.State == WebSocketState.Open || _webSocket.State == WebSocketState.CloseReceived)
            {
                _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();
            }

            _webSocket.Dispose();
            _writeLock.Dispose();
        }

        base.Dispose(disposing);
    }

    private async Task<byte[]> ReceiveMessageAsync(CancellationToken cancellationToken)
    {
        using var bufferStream = new MemoryStream();

        while (true)
        {
            WebSocketReceiveResult result;
            try
            {
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(_receiveBuffer), cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (WebSocketException) when (_webSocket.State is WebSocketState.Aborted or WebSocketState.Closed or WebSocketState.CloseReceived)
            {
                return Array.Empty<byte>();
            }

            if (result.MessageType == WebSocketMessageType.Close)
            {
                return Array.Empty<byte>();
            }

            if (result.Count > 0)
            {
                bufferStream.Write(_receiveBuffer, 0, result.Count);
            }

            if (result.EndOfMessage)
            {
                break;
            }
        }

        return bufferStream.ToArray();
    }
}

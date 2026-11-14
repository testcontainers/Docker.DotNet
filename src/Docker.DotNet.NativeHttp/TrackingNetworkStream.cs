#if NET
namespace Docker.DotNet.NativeHttp;

// SocketsHttpHandler does not expose the socket of an upgraded connection, so its
// hijacked stream cannot close only its write side. As a workaround, this stream
// records its socket in the current HijackScope when a hijack request is written to
// it, which relies on SocketsHttpHandler writing a request in the asynchronous
// control flow of the caller. A cleaner solution would send hijack requests through
// a minimal HTTP/1.1 client of its own, like the legacy transport, but that client
// would need its own TLS, proxy, and authentication handling.
internal sealed class TrackingNetworkStream : NetworkStream
{
    private static readonly ConditionalWeakTable<HttpRequestMessage, TrackingNetworkStream> PendingStreams = new();

    private volatile bool _isTrackingEnabled;

    private TrackingNetworkStream(Socket socket)
        : base(socket, ownsSocket: true)
    {
    }

    public static Stream Create(Socket socket, SocketsHttpConnectionContext context)
    {
        var stream = new TrackingNetworkStream(socket);
        PendingStreams.AddOrUpdate(context.InitialRequestMessage, stream);
        return stream;
    }

    public static ValueTask<Stream> EnableTrackingAsync(SocketsHttpPlaintextStreamFilterContext context, CancellationToken cancellationToken)
    {
        // The plaintext stream filter runs once the connection is set up,
        // after the TLS handshake.
        if (PendingStreams.TryGetValue(context.InitialRequestMessage, out var stream))
        {
            PendingStreams.Remove(context.InitialRequestMessage);
            stream._isTrackingEnabled = true;
        }

        return new ValueTask<Stream>(context.PlaintextStream);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        TrackSocket();
        base.Write(buffer, offset, count);
    }

    public override void Write(ReadOnlySpan<byte> buffer)
    {
        TrackSocket();
        base.Write(buffer);
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        TrackSocket();
        return base.WriteAsync(buffer, offset, count, cancellationToken);
    }

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        TrackSocket();
        return base.WriteAsync(buffer, cancellationToken);
    }

    private void TrackSocket()
    {
        // The connection setup runs in the scope of the request that started it, even
        // if that request is sent over another connection, so setup writes do not count.
        if (_isTrackingEnabled)
        {
            HijackScope.Current?.TrackSocket(Socket);
        }
    }
}
#endif
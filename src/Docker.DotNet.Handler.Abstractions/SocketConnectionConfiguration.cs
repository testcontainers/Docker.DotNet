namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// Configuration options for socket connections used by Docker handler factories.
/// Applies to Unix domain socket and TCP socket connections.
/// </summary>
public class SocketConnectionConfiguration
{
    /// <summary>
    /// Gets the default socket connection configuration.
    /// </summary>
    public static SocketConnectionConfiguration Default { get; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether TCP keep-alive is enabled on the socket.
    /// Default is <c>true</c>.
    /// </summary>
    public bool KeepAlive { get; set; } = true;

#if NET5_0_OR_GREATER
    /// <summary>
    /// Gets or sets the duration of time a connection can remain idle before the first
    /// keep-alive probe is sent (in seconds). Default is 30 seconds.
    /// </summary>
    public int KeepAliveTime { get; set; } = 30;

    /// <summary>
    /// Gets or sets the duration of time between keep-alive probes (in seconds).
    /// Default is 10 seconds.
    /// </summary>
    public int KeepAliveInterval { get; set; } = 10;
#endif

#if NET7_0_OR_GREATER
    /// <summary>
    /// Gets or sets the number of keep-alive probes to send before the connection is
    /// considered dead. Default is 3.
    /// </summary>
    public int KeepAliveRetryCount { get; set; } = 3;
#endif

    /// <summary>
    /// Gets or sets a value indicating whether the Nagle algorithm is disabled on the socket.
    /// Default is <c>true</c> (Nagle disabled for lower latency).
    /// </summary>
    public bool NoDelay { get; set; } = true;

    /// <summary>
    /// Gets or sets the send buffer size in bytes.
    /// A value of <c>null</c> uses the system default.
    /// </summary>
    public int? SendBufferSize { get; set; }

    /// <summary>
    /// Gets or sets the receive buffer size in bytes.
    /// A value of <c>null</c> uses the system default.
    /// </summary>
    public int? ReceiveBufferSize { get; set; }

    /// <summary>
    /// Applies this configuration to the specified socket. TCP-specific options
    /// (NoDelay, keep-alive timing) are only applied to TCP sockets.
    /// </summary>
    /// <param name="socket">The socket to configure.</param>
    public void Apply(Socket socket)
    {
        if (socket == null)
        {
            throw new ArgumentNullException(nameof(socket));
        }

        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, KeepAlive);

        // TCP-specific options are not supported on Unix domain sockets or other
        // non-TCP transports. Guard them behind a protocol type check.
        if (socket.ProtocolType == ProtocolType.Tcp)
        {
            socket.NoDelay = NoDelay;

#if NET5_0_OR_GREATER
            if (KeepAlive)
            {
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveTime, KeepAliveTime);
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveInterval, KeepAliveInterval);
            }
#endif

#if NET7_0_OR_GREATER
            if (KeepAlive)
            {
                socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.TcpKeepAliveRetryCount, KeepAliveRetryCount);
            }
#endif
        }

        if (SendBufferSize.HasValue)
        {
            socket.SendBufferSize = SendBufferSize.Value;
        }

        if (ReceiveBufferSize.HasValue)
        {
            socket.ReceiveBufferSize = ReceiveBufferSize.Value;
        }
    }
}

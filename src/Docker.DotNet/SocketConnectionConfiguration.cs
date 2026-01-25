namespace Docker.DotNet;

using System;

/// <summary>
/// Configuration options for socket connections.
/// These settings help improve proxy compatibility and connection reliability.
/// </summary>
public sealed class SocketConnectionConfiguration
{
    /// <summary>
    /// Default socket connection configuration with sensible defaults for Docker socket connections.
    /// </summary>
    public static SocketConnectionConfiguration Default { get; } = new SocketConnectionConfiguration();

    /// <summary>
    /// Gets or sets whether TCP keep-alive is enabled.
    /// Default is true for better proxy compatibility.
    /// </summary>
    public bool KeepAlive { get; set; } = true;

    /// <summary>
    /// Gets or sets the time (in seconds) the connection needs to remain idle
    /// before TCP starts sending keep-alive probes.
    /// Only applies when <see cref="KeepAlive"/> is true.
    /// Default is 30 seconds.
    /// </summary>
    /// <remarks>
    /// This setting requires .NET 5.0+ for TCP sockets. On Unix sockets, this is ignored.
    /// </remarks>
    public int KeepAliveTime { get; set; } = 30;

    /// <summary>
    /// Gets or sets the interval (in seconds) between keep-alive probes.
    /// Only applies when <see cref="KeepAlive"/> is true.
    /// Default is 10 seconds.
    /// </summary>
    /// <remarks>
    /// This setting requires .NET 5.0+ for TCP sockets. On Unix sockets, this is ignored.
    /// </remarks>
    public int KeepAliveInterval { get; set; } = 10;

    /// <summary>
    /// Gets or sets the number of keep-alive probes to send before considering the connection dead.
    /// Only applies when <see cref="KeepAlive"/> is true.
    /// Default is 3 retries.
    /// </summary>
    /// <remarks>
    /// This setting requires .NET 7.0+ for TCP sockets. On Unix sockets, this is ignored.
    /// </remarks>
    public int KeepAliveRetryCount { get; set; } = 3;

    /// <summary>
    /// Gets or sets whether to disable the Nagle algorithm (TCP_NODELAY).
    /// Default is true for lower latency with Docker API calls.
    /// </summary>
    /// <remarks>
    /// Disabling Nagle's algorithm reduces latency for small packets at the cost of
    /// potentially increased network traffic. This is generally desirable for Docker API
    /// communication which consists of many small request/response pairs.
    /// </remarks>
    public bool NoDelay { get; set; } = true;

    /// <summary>
    /// Gets or sets the socket send buffer size in bytes.
    /// Default is null (uses system default).
    /// </summary>
    public int? SendBufferSize { get; set; }

    /// <summary>
    /// Gets or sets the socket receive buffer size in bytes.
    /// Default is null (uses system default).
    /// </summary>
    public int? ReceiveBufferSize { get; set; }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Gets or sets whether Happy Eyeballs (RFC 8305) is enabled for dual-stack hosts.
    /// When enabled, IPv6 and IPv4 connections are raced to find the fastest route.
    /// Default is true.
    /// </summary>
    /// <remarks>
    /// Happy Eyeballs improves connection times on dual-stack networks by attempting
    /// IPv6 first, then starting an IPv4 connection after a short delay if IPv6
    /// hasn't connected yet, and using whichever connects first.
    /// </remarks>
    public bool EnableHappyEyeballs { get; set; } = true;

    /// <summary>
    /// Gets or sets the delay before starting an IPv4 connection when Happy Eyeballs is enabled.
    /// Default is 250ms as recommended by RFC 8305.
    /// </summary>
    public TimeSpan HappyEyeballsDelay { get; set; } = TimeSpan.FromMilliseconds(250);
#endif

    /// <summary>
    /// Applies the configuration to a socket.
    /// </summary>
    /// <param name="socket">The socket to configure.</param>
    /// <exception cref="ArgumentNullException">Thrown when socket is null.</exception>
    public void ApplyTo(System.Net.Sockets.Socket socket)
    {
        if (socket == null)
        {
            throw new ArgumentNullException(nameof(socket));
        }

        // Apply KeepAlive - works on all platforms
        if (KeepAlive)
        {
            socket.SetSocketOption(
                System.Net.Sockets.SocketOptionLevel.Socket,
                System.Net.Sockets.SocketOptionName.KeepAlive,
                true);
        }

        // Apply TCP-specific options only for TCP sockets (not Unix domain sockets)
        if (socket.ProtocolType == System.Net.Sockets.ProtocolType.Tcp)
        {
            if (NoDelay)
            {
                socket.NoDelay = true;
            }

#if NET5_0_OR_GREATER
            // TCP KeepAlive time and interval (requires .NET 5.0+)
            if (KeepAlive)
            {
                try
                {
                    socket.SetSocketOption(
                        System.Net.Sockets.SocketOptionLevel.Tcp,
                        System.Net.Sockets.SocketOptionName.TcpKeepAliveTime,
                        KeepAliveTime);

                    socket.SetSocketOption(
                        System.Net.Sockets.SocketOptionLevel.Tcp,
                        System.Net.Sockets.SocketOptionName.TcpKeepAliveInterval,
                        KeepAliveInterval);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    // These options may not be available on all platforms, ignore failures
                }
            }
#endif

#if NET7_0_OR_GREATER
            // TCP KeepAlive retry count (requires .NET 7.0+)
            if (KeepAlive)
            {
                try
                {
                    socket.SetSocketOption(
                        System.Net.Sockets.SocketOptionLevel.Tcp,
                        System.Net.Sockets.SocketOptionName.TcpKeepAliveRetryCount,
                        KeepAliveRetryCount);
                }
                catch (System.Net.Sockets.SocketException)
                {
                    // This option may not be available on all platforms, ignore failures
                }
            }
#endif
        }

        // Apply buffer sizes if specified
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

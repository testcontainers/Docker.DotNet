#if NET5_0_OR_GREATER
namespace Microsoft.Net.Http.Client;

/// <summary>
/// Manages a pool of HTTP connections for reuse.
/// </summary>
internal sealed class ConnectionPool : IDisposable
{
    private readonly ConcurrentDictionary<string, ConcurrentQueue<PooledConnection>> _pools = new();
    private readonly ILogger _logger;
    private readonly Timer _cleanupTimer;
    private bool _disposed;

    /// <summary>
    /// Gets or sets the maximum time a connection can remain idle in the pool before being removed.
    /// Default is 2 minutes.
    /// </summary>
    public TimeSpan IdleTimeout { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets or sets the maximum lifetime of a connection, regardless of activity.
    /// Default is 10 minutes.
    /// </summary>
    public TimeSpan ConnectionLifetime { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the maximum number of connections per host.
    /// Default is 10.
    /// </summary>
    public int MaxConnectionsPerHost { get; set; } = 10;

    public ConnectionPool(ILogger logger)
    {
        _logger = logger;
        // Run cleanup every 30 seconds
        _cleanupTimer = new Timer(CleanupCallback, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
    }

    /// <summary>
    /// Attempts to get a pooled connection for the specified host and port.
    /// </summary>
    /// <param name="host">The host name.</param>
    /// <param name="port">The port number.</param>
    /// <param name="isHttps">Whether the connection uses HTTPS.</param>
    /// <param name="connection">The pooled connection if found.</param>
    /// <returns>True if a valid pooled connection was found; otherwise, false.</returns>
    public bool TryGetConnection(string host, int port, bool isHttps, out PooledConnection connection)
    {
        connection = null;

        if (_disposed)
        {
            return false;
        }

        var key = GetPoolKey(host, port, isHttps);

        if (!_pools.TryGetValue(key, out var pool))
        {
            return false;
        }

        while (pool.TryDequeue(out var candidate))
        {
            // Check if connection is still valid
            if (IsConnectionValid(candidate))
            {
                connection = candidate;
                _logger.LogDebug("Reusing pooled connection to {Host}:{Port}", host, port);
                return true;
            }

            // Connection is no longer valid, dispose it
            try
            {
                candidate.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error disposing invalid pooled connection");
            }
        }

        return false;
    }

    /// <summary>
    /// Returns a connection to the pool for reuse.
    /// </summary>
    /// <param name="connection">The connection to return.</param>
    /// <returns>True if the connection was returned to the pool; otherwise, false.</returns>
    public bool ReturnConnection(PooledConnection connection)
    {
        if (_disposed || connection == null)
        {
            connection?.Dispose();
            return false;
        }

        // Check if connection is still valid for reuse
        if (!IsConnectionValid(connection))
        {
            _logger.LogDebug("Connection not valid for pooling, disposing");
            connection.Dispose();
            return false;
        }

        var key = connection.PoolKey;
        var pool = _pools.GetOrAdd(key, _ => new ConcurrentQueue<PooledConnection>());

        // Check pool size limit
        if (pool.Count >= MaxConnectionsPerHost)
        {
            _logger.LogDebug("Pool for {Key} is full ({Count}/{Max}), disposing connection",
                key, pool.Count, MaxConnectionsPerHost);
            connection.Dispose();
            return false;
        }

        connection.LastUsed = DateTime.UtcNow;
        pool.Enqueue(connection);
        _logger.LogDebug("Returned connection to pool for {Key}", key);
        return true;
    }

    /// <summary>
    /// Creates a new pooled connection wrapper.
    /// </summary>
    public PooledConnection CreatePooledConnection(
        string host,
        int port,
        bool isHttps,
        Socket socket,
        Stream transport,
        BufferedReadStream bufferedStream)
    {
        var key = GetPoolKey(host, port, isHttps);
        return new PooledConnection(key, socket, transport, bufferedStream);
    }

    private bool IsConnectionValid(PooledConnection connection)
    {
        // Check lifetime
        if (DateTime.UtcNow - connection.Created > ConnectionLifetime)
        {
            return false;
        }

        // Check idle timeout
        if (DateTime.UtcNow - connection.LastUsed > IdleTimeout)
        {
            return false;
        }

        // Check if socket is still connected
        if (connection.Socket != null)
        {
            try
            {
                // Poll with zero timeout to check socket state
                // SelectRead with available=0 means the connection was closed gracefully
                if (connection.Socket.Poll(0, SelectMode.SelectRead))
                {
                    if (connection.Socket.Available == 0)
                    {
                        return false; // Connection was closed
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        return true;
    }

    private static string GetPoolKey(string host, int port, bool isHttps)
    {
        return $"{(isHttps ? "https" : "http")}://{host}:{port}";
    }

    private void CleanupCallback(object state)
    {
        if (_disposed)
        {
            return;
        }

        foreach (var kvp in _pools)
        {
            var pool = kvp.Value;
            var validConnections = new List<PooledConnection>();

            while (pool.TryDequeue(out var connection))
            {
                if (IsConnectionValid(connection))
                {
                    validConnections.Add(connection);
                }
                else
                {
                    try
                    {
                        connection.Dispose();
                        _logger.LogDebug("Cleaned up expired connection from pool {Key}", kvp.Key);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error disposing expired pooled connection");
                    }
                }
            }

            // Re-add valid connections
            foreach (var conn in validConnections)
            {
                pool.Enqueue(conn);
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _cleanupTimer.Dispose();

        foreach (var kvp in _pools)
        {
            while (kvp.Value.TryDequeue(out var connection))
            {
                try
                {
                    connection.Dispose();
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }

        _pools.Clear();
    }
}

/// <summary>
/// Represents a pooled connection that can be returned to the pool for reuse.
/// </summary>
internal sealed class PooledConnection : IDisposable
{
    private bool _disposed;

    public PooledConnection(string poolKey, Socket socket, Stream transport, BufferedReadStream bufferedStream)
    {
        PoolKey = poolKey;
        Socket = socket;
        Transport = transport;
        BufferedStream = bufferedStream;
        Created = DateTime.UtcNow;
        LastUsed = DateTime.UtcNow;
    }

    public string PoolKey { get; }
    public Socket Socket { get; }
    public Stream Transport { get; }
    public BufferedReadStream BufferedStream { get; }
    public DateTime Created { get; }
    public DateTime LastUsed { get; set; }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        try
        {
            BufferedStream?.Dispose();
        }
        catch
        {
            // Ignore
        }
    }
}
#endif

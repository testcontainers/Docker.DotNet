namespace Microsoft.Net.Http.Client;

public class HttpConnectionResponseContent : HttpContent
{
    private readonly HttpConnection _connection;
    private Stream _responseStream;
    private bool _hijacked;

#if NET5_0_OR_GREATER
    private PooledConnection _pooledConnection;
    private Action<PooledConnection> _poolReturnCallback;
#endif

    internal HttpConnectionResponseContent(HttpConnection connection)
    {
        _connection = connection;
    }

#if NET5_0_OR_GREATER
    internal HttpConnectionResponseContent(HttpConnection connection, PooledConnection pooledConnection, Action<PooledConnection> poolReturnCallback)
    {
        _connection = connection;
        _pooledConnection = pooledConnection;
        _poolReturnCallback = poolReturnCallback;
    }

    internal void SetPoolReturnCallback(PooledConnection pooledConnection, Action<PooledConnection> poolReturnCallback)
    {
        _pooledConnection = pooledConnection;
        _poolReturnCallback = poolReturnCallback;
    }
#endif

    internal void ResolveResponseStream(bool chunked, bool isConnectionUpgrade)
    {
        if (_responseStream != null)
        {
            throw new InvalidOperationException("Called multiple times");
        }
        if (chunked)
        {
            _responseStream = new ChunkedReadStream(_connection.Transport);
        }
        else if (!isConnectionUpgrade && Headers.ContentLength.HasValue)
        {
            _responseStream = new ContentLengthReadStream(_connection.Transport, Headers.ContentLength.Value);
        }
        else
        {
            _responseStream = _connection.Transport;
        }
    }

    public WriteClosableStream HijackStream()
    {
        if (_responseStream != _connection.Transport)
        {
            throw new InvalidOperationException("cannot hijack chunked or content length stream");
        }

        _hijacked = true;

#if NET5_0_OR_GREATER
        // Hijacked connections cannot be returned to the pool
        _poolReturnCallback = null;
        _pooledConnection = null;
#endif

        return _connection.Transport;
    }

    protected override Task SerializeToStreamAsync(Stream stream, System.Net.TransportContext context)
    {
        return _responseStream.CopyToAsync(stream);
    }

    protected override Task<Stream> CreateContentReadStreamAsync()
    {
        return Task.FromResult(_responseStream);
    }

    protected override bool TryComputeLength(out long length)
    {
        length = 0;
        return false;
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            if (disposing)
            {
#if NET5_0_OR_GREATER
                // Try to return connection to pool if not hijacked
                if (!_hijacked && _poolReturnCallback != null && _pooledConnection != null)
                {
                    try
                    {
                        // Drain any remaining response data before returning to pool
                        // Only attempt for content-length streams where we know the remaining bytes
                        if (_responseStream is ContentLengthReadStream)
                        {
                            // Pool return callback will handle disposal if pooling fails
                            _poolReturnCallback(_pooledConnection);
                            _pooledConnection = null;
                            _poolReturnCallback = null;
                            return; // Don't dispose the connection, it's returned to pool
                        }
                    }
                    catch
                    {
                        // Fall through to normal disposal
                    }
                }
#endif

                _responseStream?.Dispose();
                _connection.Dispose();
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }
}
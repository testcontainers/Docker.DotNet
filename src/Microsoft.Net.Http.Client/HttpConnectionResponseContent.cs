namespace Microsoft.Net.Http.Client;

public class HttpConnectionResponseContent : HttpContent
{
    private readonly HttpConnection _connection;
    private readonly CancellationToken _cancellationToken;
    private Stream _responseStream;

    internal HttpConnectionResponseContent(HttpConnection connection, CancellationToken cancellationToken)
    {
        _connection = connection;
        _cancellationToken = cancellationToken;
    }

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

        return _connection.Transport;
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
        return _responseStream.CopyToAsync(stream, 81920, _cancellationToken);
    }

#if NET6_0_OR_GREATER
    protected override Task SerializeToStreamAsync(Stream stream, TransportContext context, CancellationToken cancellationToken)
    {
        return _responseStream.CopyToAsync(stream, cancellationToken);
    }
#endif

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
                _responseStream.Dispose();
                _connection.Dispose();
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }
}
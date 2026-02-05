using System.Net.Http.Headers;

namespace Docker.DotNet.Tests;

public class ManagedHandlerProxyTests
{
    private static ManagedHandler CreateStreamHandler(ManagedHandler.StreamOpener opener)
    {
        return new ManagedHandler(opener, NullLogger.Instance);
    }

    private static ManagedHandler CreateStreamHandler(ManagedHandler.StreamOpener opener, IWebProxy proxy)
    {
        var handler = new ManagedHandler(opener, NullLogger.Instance)
        {
            Proxy = proxy,
            UseProxy = true,
        };
        return handler;
    }

    #region DuplexStream helper

    private sealed class DuplexStream : Stream
    {
        private readonly MemoryStream _readStream;
        private readonly MemoryStream _writeStream = new();

        public DuplexStream(string httpResponse)
        {
            _readStream = new MemoryStream(Encoding.ASCII.GetBytes(httpResponse));
        }

        public string WrittenAsString => Encoding.UTF8.GetString(_writeStream.ToArray());

        public bool IsDisposed { get; private set; }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _readStream.Read(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _readStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _writeStream.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _writeStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }

    #endregion

    #region NullCredentials helper

    private sealed class NullCredentials : ICredentials
    {
        public NetworkCredential GetCredential(Uri uri, string authType) => null;
    }

    #endregion

    private static string BuildHttpResponse(int statusCode, string reasonPhrase, string extraHeaders = "")
    {
        return $"HTTP/1.1 {statusCode} {reasonPhrase}\r\nContent-Length: 0\r\n{extraHeaders}\r\n";
    }

    private static HttpRequestMessage CreateHttpRequest(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        return request;
    }

    // ────────────────────────────────────────────────
    // A. GetProxyAuthorizationHeader (4 tests)
    // ────────────────────────────────────────────────

    [Fact]
    public void GetProxyAuthorizationHeader_WithCredentials_ReturnsBasicAuth()
    {
        var proxy = new WebProxy("http://proxy.local:8080")
        {
            Credentials = new NetworkCredential("user", "pass"),
        };

        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()), proxy);

        var result = handler.GetProxyAuthorizationHeader(new Uri("http://proxy.local:8080"));

        Assert.NotNull(result);
        Assert.Equal("Basic", result.Scheme);

        var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes("user:pass"));
        Assert.Equal(expected, result.Parameter);
    }

    [Fact]
    public void GetProxyAuthorizationHeader_NoCredentials_ReturnsNull()
    {
        var proxy = new WebProxy("http://proxy.local:8080");

        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()), proxy);

        var result = handler.GetProxyAuthorizationHeader(new Uri("http://proxy.local:8080"));

        Assert.Null(result);
    }

    [Fact]
    public void GetProxyAuthorizationHeader_NullProxy_ReturnsNull()
    {
        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()));
        handler.UseProxy = false;
        handler.Proxy = null;

        var result = handler.GetProxyAuthorizationHeader(new Uri("http://proxy.local:8080"));

        Assert.Null(result);
    }

    [Fact]
    public void GetProxyAuthorizationHeader_GetCredentialReturnsNull_ReturnsNull()
    {
        var proxy = new WebProxy("http://proxy.local:8080")
        {
            Credentials = new NullCredentials(),
        };

        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()), proxy);

        var result = handler.GetProxyAuthorizationHeader(new Uri("http://proxy.local:8080"));

        Assert.Null(result);
    }

    // ────────────────────────────────────────────────
    // B. HTTP-mode preemptive auth (4 tests via SendAsync)
    // ────────────────────────────────────────────────

    [Fact]
    public async Task SendAsync_HttpProxy_WithCredentials_SetsProxyAuthorization()
    {
        var response = BuildHttpResponse(200, "OK");
        DuplexStream transport = null;

        var proxy = new WebProxy("http://proxy.local:3128")
        {
            Credentials = new NetworkCredential("alice", "secret"),
        };

        var handler = CreateStreamHandler((_, _, _) =>
        {
            transport = new DuplexStream(response);
            return Task.FromResult<Stream>(transport);
        }, proxy);

        using var client = new HttpClient(handler);
        using var result = await client.GetAsync("http://target.example.com/api");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes("alice:secret"));
        Assert.Contains($"Proxy-Authorization: Basic {expected}", transport.WrittenAsString);
    }

    [Fact]
    public async Task SendAsync_HttpProxy_NoCredentials_NoProxyAuthorizationHeader()
    {
        var response = BuildHttpResponse(200, "OK");
        DuplexStream transport = null;

        var proxy = new WebProxy("http://proxy.local:3128");

        var handler = CreateStreamHandler((_, _, _) =>
        {
            transport = new DuplexStream(response);
            return Task.FromResult<Stream>(transport);
        }, proxy);

        using var client = new HttpClient(handler);
        using var result = await client.GetAsync("http://target.example.com/api");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.DoesNotContain("Proxy-Authorization", transport.WrittenAsString);
    }

    [Fact]
    public async Task SendAsync_HttpProxy_ExistingProxyAuthorization_NotOverwritten()
    {
        var response = BuildHttpResponse(200, "OK");
        DuplexStream transport = null;

        var proxy = new WebProxy("http://proxy.local:3128")
        {
            Credentials = new NetworkCredential("alice", "secret"),
        };

        var handler = CreateStreamHandler((_, _, _) =>
        {
            transport = new DuplexStream(response);
            return Task.FromResult<Stream>(transport);
        }, proxy);

        using var invoker = new HttpMessageInvoker(handler);
        var request = CreateHttpRequest("http://target.example.com/api");
        request.Headers.ProxyAuthorization = new AuthenticationHeaderValue("Bearer", "my-token");

        using var result = await invoker.SendAsync(request, CancellationToken.None);

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Contains("Proxy-Authorization: Bearer my-token", transport.WrittenAsString);
        // Should NOT contain the Basic auth from the proxy credentials
        var basicEncoded = Convert.ToBase64String(Encoding.ASCII.GetBytes("alice:secret"));
        Assert.DoesNotContain($"Proxy-Authorization: Basic {basicEncoded}", transport.WrittenAsString);
    }

    [Fact]
    public async Task SendAsync_NoProxy_NoProxyAuthorizationHeader()
    {
        var response = BuildHttpResponse(200, "OK");
        DuplexStream transport = null;

        var handler = CreateStreamHandler((_, _, _) =>
        {
            transport = new DuplexStream(response);
            return Task.FromResult<Stream>(transport);
        });
        handler.UseProxy = false;

        using var client = new HttpClient(handler);
        using var result = await client.GetAsync("http://target.example.com/api");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.DoesNotContain("Proxy-Authorization", transport.WrittenAsString);
    }

    // ────────────────────────────────────────────────
    // C. CONNECT tunnel auth (3 tests via TunnelThroughProxyAsync)
    // ────────────────────────────────────────────────

    private static HttpRequestMessage CreateTunnelRequest()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://target.example.com:443/path");
        request.SetSchemeProperty("https");
        request.SetHostProperty("target.example.com");
        request.SetPortProperty(443);
        request.SetConnectionHostProperty("proxy.local");
        request.SetConnectionPortProperty(3128);
        request.SetPathAndQueryProperty("/path");
        return request;
    }

    [Fact]
    public async Task Tunnel_WithCredentials_SetsProxyAuthorizationOnConnect()
    {
        var connectResponse = "HTTP/1.1 200 Connection Established\r\n\r\n";
        var transport = new DuplexStream(connectResponse);

        var proxy = new WebProxy("http://proxy.local:3128")
        {
            Credentials = new NetworkCredential("bob", "pass123"),
        };

        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()), proxy);

        var request = CreateTunnelRequest();

        var (resultTransport, _) = await handler.TunnelThroughProxyAsync(request, transport, null, CancellationToken.None);

        var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes("bob:pass123"));
        Assert.Contains($"Proxy-Authorization: Basic {expected}", transport.WrittenAsString);
        Assert.Same(transport, resultTransport);
    }

    [Fact]
    public async Task Tunnel_ExistingRequestAuth_PreservesIt()
    {
        var connectResponse = "HTTP/1.1 200 Connection Established\r\n\r\n";
        var transport = new DuplexStream(connectResponse);

        var proxy = new WebProxy("http://proxy.local:3128")
        {
            Credentials = new NetworkCredential("bob", "pass123"),
        };

        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()), proxy);

        var request = CreateTunnelRequest();
        request.Headers.ProxyAuthorization = new AuthenticationHeaderValue("Bearer", "caller-token");

        var (resultTransport, _) = await handler.TunnelThroughProxyAsync(request, transport, null, CancellationToken.None);

        Assert.Contains("Proxy-Authorization: Bearer caller-token", transport.WrittenAsString);
        // Should use the caller's auth, not the proxy credentials
        var basicEncoded = Convert.ToBase64String(Encoding.ASCII.GetBytes("bob:pass123"));
        Assert.DoesNotContain($"Proxy-Authorization: Basic {basicEncoded}", transport.WrittenAsString);
    }

    [Fact]
    public async Task Tunnel_NoCredentials_ConnectHasNoAuthHeader()
    {
        var connectResponse = "HTTP/1.1 200 Connection Established\r\n\r\n";
        var transport = new DuplexStream(connectResponse);

        var proxy = new WebProxy("http://proxy.local:3128");

        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()), proxy);

        var request = CreateTunnelRequest();

        var (resultTransport, _) = await handler.TunnelThroughProxyAsync(request, transport, null, CancellationToken.None);

        Assert.DoesNotContain("Proxy-Authorization", transport.WrittenAsString);
        Assert.Same(transport, resultTransport);
    }

    // ────────────────────────────────────────────────
    // D. 407 retry logic (4 tests via TunnelThroughProxyAsync)
    // ────────────────────────────────────────────────

    [Fact]
    public async Task Tunnel_407_RetriesWithCredentials()
    {
        var firstResponse = "HTTP/1.1 407 Proxy Authentication Required\r\nContent-Length: 0\r\n\r\n";
        var retryResponse = "HTTP/1.1 200 Connection Established\r\n\r\n";
        var firstTransport = new DuplexStream(firstResponse);

        DuplexStream secondTransport = null;

        var proxy = new WebProxy("http://proxy.local:3128")
        {
            Credentials = new NetworkCredential("bob", "pass123"),
        };

        var handler = CreateStreamHandler((_, _, _) =>
        {
            secondTransport = new DuplexStream(retryResponse);
            return Task.FromResult<Stream>(secondTransport);
        }, proxy);

        var request = CreateTunnelRequest();

        var (resultTransport, _) = await handler.TunnelThroughProxyAsync(request, firstTransport, null, CancellationToken.None);

        // The first transport should be disposed after the 407
        Assert.True(firstTransport.IsDisposed);

        // The retry should contain the proxy auth
        Assert.NotNull(secondTransport);
        var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes("bob:pass123"));
        Assert.Contains($"Proxy-Authorization: Basic {expected}", secondTransport.WrittenAsString);

        // Result should be the second transport
        Assert.Same(secondTransport, resultTransport);
    }

    [Fact]
    public async Task Tunnel_407_NoCredentials_Throws()
    {
        var response407 = "HTTP/1.1 407 Proxy Authentication Required\r\nContent-Length: 0\r\n\r\n";
        var transport = new DuplexStream(response407);

        var proxy = new WebProxy("http://proxy.local:3128");

        var handler = CreateStreamHandler((_, _, _) => Task.FromResult<Stream>(new MemoryStream()), proxy);

        var request = CreateTunnelRequest();

        var ex = await Assert.ThrowsAsync<HttpRequestException>(
            () => handler.TunnelThroughProxyAsync(request, transport, null, CancellationToken.None));

        Assert.Contains("no credentials were provided", ex.Message);
    }

    [Fact]
    public async Task Tunnel_407_RetryNon2xx_Throws()
    {
        var firstResponse = "HTTP/1.1 407 Proxy Authentication Required\r\nContent-Length: 0\r\n\r\n";
        var retryResponse = "HTTP/1.1 403 Forbidden\r\nContent-Length: 0\r\n\r\n";

        var firstTransport = new DuplexStream(firstResponse);

        var proxy = new WebProxy("http://proxy.local:3128")
        {
            Credentials = new NetworkCredential("bob", "pass123"),
        };

        var handler = CreateStreamHandler((_, _, _) =>
        {
            return Task.FromResult<Stream>(new DuplexStream(retryResponse));
        }, proxy);

        var request = CreateTunnelRequest();

        var ex = await Assert.ThrowsAsync<HttpRequestException>(
            () => handler.TunnelThroughProxyAsync(request, firstTransport, null, CancellationToken.None));

        Assert.Contains("after authentication", ex.Message);
    }

    [Fact]
    public async Task Tunnel_407_DisposesOriginalTransport()
    {
        var firstResponse = "HTTP/1.1 407 Proxy Authentication Required\r\nContent-Length: 0\r\n\r\n";
        var retryResponse = "HTTP/1.1 200 Connection Established\r\n\r\n";

        var firstTransport = new DuplexStream(firstResponse);

        var proxy = new WebProxy("http://proxy.local:3128")
        {
            Credentials = new NetworkCredential("bob", "pass123"),
        };

        var handler = CreateStreamHandler((_, _, _) =>
        {
            return Task.FromResult<Stream>(new DuplexStream(retryResponse));
        }, proxy);

        var request = CreateTunnelRequest();

        await handler.TunnelThroughProxyAsync(request, firstTransport, null, CancellationToken.None);

        Assert.True(firstTransport.IsDisposed);
    }
}

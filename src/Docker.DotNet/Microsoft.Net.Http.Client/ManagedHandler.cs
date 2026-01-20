namespace Microsoft.Net.Http.Client;

using System;
using Docker.DotNet;

public class ManagedHandler : HttpMessageHandler
{
    private readonly ILogger _logger;

    private readonly StreamOpener _streamOpener;

    private readonly SocketOpener _socketOpener;

    private readonly SocketConfiguration _socketConfiguration;

    private IWebProxy _proxy;

#if NET5_0_OR_GREATER
    private readonly ConnectionPool _connectionPool;
    private bool _enableConnectionPooling = true;
#endif

    public delegate Task<Stream> StreamOpener(string host, int port, CancellationToken cancellationToken);

    public delegate Task<Socket> SocketOpener(string host, int port, CancellationToken cancellationToken);

    public ManagedHandler(ILogger logger)
        : this(logger, SocketConfiguration.Default)
    {
    }

    public ManagedHandler(ILogger logger, SocketConfiguration socketConfiguration)
    {
        _logger = logger;
        _socketConfiguration = socketConfiguration ?? SocketConfiguration.Default;
        _socketOpener = TcpSocketOpenerAsync;
#if NET5_0_OR_GREATER
        _connectionPool = new ConnectionPool(logger);
#endif
    }

    public ManagedHandler(StreamOpener opener, ILogger logger)
    {
        _logger = logger;
        _socketConfiguration = SocketConfiguration.Default;
        _streamOpener = opener ?? throw new ArgumentNullException(nameof(opener));
#if NET5_0_OR_GREATER
        _connectionPool = new ConnectionPool(logger);
#endif
    }

    public ManagedHandler(SocketOpener opener, ILogger logger)
    {
        _logger = logger;
        _socketConfiguration = SocketConfiguration.Default;
        _socketOpener = opener ?? throw new ArgumentNullException(nameof(opener));
#if NET5_0_OR_GREATER
        _connectionPool = new ConnectionPool(logger);
#endif
    }

    public ManagedHandler(SocketOpener opener, ILogger logger, SocketConfiguration socketConfiguration)
    {
        _logger = logger;
        _socketConfiguration = socketConfiguration ?? SocketConfiguration.Default;
        _socketOpener = opener ?? throw new ArgumentNullException(nameof(opener));
#if NET5_0_OR_GREATER
        _connectionPool = new ConnectionPool(logger);
#endif
    }

    public IWebProxy Proxy
    {
        get
        {
            if (_proxy == null)
            {
#if NET6_0_OR_GREATER
                // Use modern HttpClient.DefaultProxy for better proxy resolution
                _proxy = HttpClient.DefaultProxy;
#else
                _proxy = WebRequest.DefaultWebProxy;
#endif
            }

            return _proxy;
        }
        set
        {
            _proxy = value;
        }
    }

    public bool UseProxy { get; set; } = true;

    public int MaxAutomaticRedirects { get; set; } = 20;

    public RedirectMode RedirectMode { get; set; } = RedirectMode.NoDowngrade;

    public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

    public X509CertificateCollection ClientCertificates { get; set; } = new X509Certificate2Collection();

    /// <summary>
    /// Gets or sets the connection timeout. Default is 30 seconds.
    /// Set to <see cref="Timeout.InfiniteTimeSpan"/> to disable connection timeout.
    /// </summary>
    public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(30);

#if NET5_0_OR_GREATER
    /// <summary>
    /// Gets or sets whether connection pooling is enabled.
    /// Default is true. When enabled, connections are reused for subsequent requests.
    /// Connection pooling is automatically disabled for hijacking requests (attach/exec).
    /// </summary>
    public bool EnableConnectionPooling
    {
        get => _enableConnectionPooling;
        set => _enableConnectionPooling = value;
    }

    /// <summary>
    /// Gets the connection pool used by this handler.
    /// </summary>
    internal ConnectionPool ConnectionPool => _connectionPool;
#endif

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
    {
        if (httpRequestMessage == null)
        {
            throw new ArgumentNullException(nameof(httpRequestMessage));
        }

        HttpResponseMessage httpResponseMessage = null;

        for (var i = 0; i < MaxAutomaticRedirects; i++)
        {
            httpResponseMessage?.Dispose();

            httpResponseMessage = await ProcessRequestAsync(httpRequestMessage, cancellationToken)
                .ConfigureAwait(false);

            if (!IsRedirectResponse(httpRequestMessage, httpResponseMessage))
            {
                return httpResponseMessage;
            }
        }

        return httpResponseMessage;
    }

    private bool IsRedirectResponse(HttpRequestMessage request, HttpResponseMessage response)
    {
        if (response.StatusCode < HttpStatusCode.MovedPermanently || response.StatusCode >= HttpStatusCode.BadRequest)
        {
            return false;
        }

        if (RedirectMode == RedirectMode.None)
        {
            return false;
        }

        var location = response.Headers.Location;

        if (location == null)
        {
            return false;
        }

        if (!location.IsAbsoluteUri)
        {
            request.RequestUri = location;
            request.Headers.Authorization = null;
            request.SetAddressLineProperty(null);
            request.SetPathAndQueryProperty(null);
            return true;
        }

        // Check if redirect from https to http is allowed
        if (request.IsHttps() && string.Equals("http", location.Scheme, StringComparison.OrdinalIgnoreCase)
                              && RedirectMode == RedirectMode.NoDowngrade)
        {
            return false;
        }

        // Reset fields calculated from the URI.
        request.RequestUri = location;
        request.Headers.Authorization = null;
        request.Headers.Host = null;
        request.SetConnectionHostProperty(null);
        request.SetConnectionPortProperty(null);
        request.SetSchemeProperty(null);
        request.SetHostProperty(null);
        request.SetPortProperty(null);
        request.SetAddressLineProperty(null);
        request.SetPathAndQueryProperty(null);
        return true;
    }

    private async Task<HttpResponseMessage> ProcessRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ProcessUrl(request);
        ProcessHostHeader(request);

        // Check if this is a hijacking request (attach/exec) - these cannot use pooled connections
        var isHijackingRequest = IsHijackingRequest(request);

#if NET5_0_OR_GREATER
        // For non-hijacking requests with pooling enabled, try to get a pooled connection
        if (_enableConnectionPooling && !isHijackingRequest)
        {
            request.Headers.ConnectionClose = false; // Keep connection alive for pooling
        }
        else
        {
            request.Headers.ConnectionClose = !request.Headers.Contains("Connection");
        }
#else
        request.Headers.ConnectionClose = !request.Headers.Contains("Connection"); // TODO: Connection reuse is not supported.
#endif

        ProxyMode proxyMode = DetermineProxyModeAndAddressLine(request);
        Socket socket = null;
        Stream transport = null;
        BufferedReadStream bufferedReadStream = null;

#if NET5_0_OR_GREATER
        // Try to get a pooled connection first
        bool usePooledConnection = false;
        PooledConnection pooledConnection = null;

        if (_enableConnectionPooling && !isHijackingRequest && proxyMode == ProxyMode.None)
        {
            try
            {
                if (_connectionPool.TryGetConnection(
                    request.GetConnectionHostProperty(),
                    request.GetConnectionPortProperty().Value,
                    request.IsHttps(),
                    out pooledConnection))
                {
                    socket = pooledConnection.Socket;
                    transport = pooledConnection.Transport;
                    bufferedReadStream = pooledConnection.BufferedStream;
                    usePooledConnection = true;
                    _logger.LogDebug("Using pooled connection for {Host}:{Port}",
                        request.GetConnectionHostProperty(), request.GetConnectionPortProperty());
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting pooled connection, creating new connection");
            }
        }

        if (!usePooledConnection)
        {
#endif
            // Create linked cancellation token for connection timeout
            using var timeoutCts = ConnectTimeout != Timeout.InfiniteTimeSpan
                ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
                : null;

            if (timeoutCts != null)
            {
                timeoutCts.CancelAfter(ConnectTimeout);
            }

            var effectiveCancellationToken = timeoutCts?.Token ?? cancellationToken;

            try
            {
                if (_socketOpener != null)
                {
                    socket = await _socketOpener(request.GetConnectionHostProperty(), request.GetConnectionPortProperty().Value, effectiveCancellationToken).ConfigureAwait(false);
                    transport = new NetworkStream(socket, true);
                }
                else
                {
                    socket = null;
                    transport = await _streamOpener(request.GetConnectionHostProperty(), request.GetConnectionPortProperty().Value, effectiveCancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException) when (timeoutCts?.IsCancellationRequested == true && !cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"Connection to {request.GetConnectionHostProperty()}:{request.GetConnectionPortProperty()} timed out after {ConnectTimeout.TotalSeconds} seconds.");
            }
            catch (SocketException e)
            {
                throw new HttpRequestException("Connection failed.", e);
            }

            if (proxyMode == ProxyMode.Tunnel)
            {
                await TunnelThroughProxyAsync(request, transport, cancellationToken);
            }

            if (request.IsHttps())
            {
                transport = await EstablishSslAsync(transport, request.GetHostProperty(), cancellationToken).ConfigureAwait(false);
            }

            bufferedReadStream = new BufferedReadStream(transport, socket, _logger);
#if NET5_0_OR_GREATER
        }
#endif

        var connection = new HttpConnection(bufferedReadStream);

#if NET5_0_OR_GREATER
        // Create pool return callback for non-hijacking requests
        Action<PooledConnection> poolReturnCallback = null;
        if (_enableConnectionPooling && !isHijackingRequest && proxyMode == ProxyMode.None)
        {
            var host = request.GetConnectionHostProperty();
            var port = request.GetConnectionPortProperty().Value;
            var isHttps = request.IsHttps();

            poolReturnCallback = pooledConn =>
            {
                try
                {
                    if (!_connectionPool.ReturnConnection(pooledConn))
                    {
                        pooledConn.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error returning connection to pool");
                    pooledConn?.Dispose();
                }
            };
        }

        var responseContent = new HttpConnectionResponseContent(
            connection,
            _enableConnectionPooling && !isHijackingRequest && proxyMode == ProxyMode.None
                ? _connectionPool.CreatePooledConnection(
                    request.GetConnectionHostProperty(),
                    request.GetConnectionPortProperty().Value,
                    request.IsHttps(),
                    socket,
                    transport,
                    bufferedReadStream)
                : null,
            poolReturnCallback);

        var response = await connection.SendAsync(request, cancellationToken);

        // Replace the content with our pooling-aware content
        var originalContent = response.Content as HttpConnectionResponseContent;
        if (originalContent != null && poolReturnCallback != null)
        {
            originalContent.SetPoolReturnCallback(
                _connectionPool.CreatePooledConnection(
                    request.GetConnectionHostProperty(),
                    request.GetConnectionPortProperty().Value,
                    request.IsHttps(),
                    socket,
                    transport,
                    bufferedReadStream),
                poolReturnCallback);
        }

        return response;
#else
        return await connection.SendAsync(request, cancellationToken);
#endif
    }

    private static bool IsHijackingRequest(HttpRequestMessage request)
    {
        // Check for requests that require connection hijacking
        // These are typically attach/exec operations that upgrade the connection
        var pathAndQuery = request.GetPathAndQueryProperty() ?? request.RequestUri?.PathAndQuery ?? string.Empty;

        return pathAndQuery.Contains("/attach") ||
               pathAndQuery.Contains("/exec/") ||
               request.Headers.TryGetValues("Upgrade", out var upgradeValues) &&
               upgradeValues.Any(v => "tcp".Equals(v, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<Stream> EstablishSslAsync(Stream transport, string targetHost, CancellationToken cancellationToken)
    {
        var sslStream = new SslStream(transport, false, ServerCertificateValidationCallback);

        try
        {
#if NET5_0_OR_GREATER
            // Use modern SslClientAuthenticationOptions for better TLS configuration
            var sslOptions = new SslClientAuthenticationOptions
            {
                TargetHost = targetHost,
                ClientCertificates = ClientCertificates,
                // Let the OS choose the best protocol (TLS 1.2/1.3)
                EnabledSslProtocols = SslProtocols.None,
                CertificateRevocationCheckMode = X509RevocationMode.NoCheck
            };

            await sslStream.AuthenticateAsClientAsync(sslOptions, cancellationToken).ConfigureAwait(false);
#else
            // Fallback for older frameworks - use TLS 1.2 as minimum
            await sslStream.AuthenticateAsClientAsync(targetHost, ClientCertificates, SslProtocols.Tls12, checkCertificateRevocation: false).ConfigureAwait(false);
#endif
            return sslStream;
        }
        catch
        {
            sslStream.Dispose();
            throw;
        }
    }

    // Data comes from either the request.RequestUri or from the request.Properties
    private static void ProcessUrl(HttpRequestMessage request)
    {
        string scheme = request.GetSchemeProperty();
        if (string.IsNullOrWhiteSpace(scheme))
        {
            if (!request.RequestUri.IsAbsoluteUri)
            {
                throw new InvalidOperationException("Missing URL Scheme");
            }
            scheme = request.RequestUri.Scheme;
            request.SetSchemeProperty(scheme);
        }

        if (!request.IsHttp() && !request.IsHttps())
        {
            throw new InvalidOperationException("Only HTTP or HTTPS are supported, not: " + request.RequestUri.Scheme);
        }

        string host = request.GetHostProperty();
        if (string.IsNullOrWhiteSpace(host))
        {
            if (!request.RequestUri.IsAbsoluteUri)
            {
                throw new InvalidOperationException("Missing URL Scheme");
            }
            host = request.RequestUri.DnsSafeHost;
            request.SetHostProperty(host);
        }

        string connectionHost = request.GetConnectionHostProperty();
        if (string.IsNullOrWhiteSpace(connectionHost))
        {
            request.SetConnectionHostProperty(host);
        }

        int? port = request.GetPortProperty();
        if (!port.HasValue)
        {
            if (!request.RequestUri.IsAbsoluteUri)
            {
                throw new InvalidOperationException("Missing URL Scheme");
            }
            port = request.RequestUri.Port;
            request.SetPortProperty(port);
        }

        int? connectionPort = request.GetConnectionPortProperty();
        if (!connectionPort.HasValue)
        {
            request.SetConnectionPortProperty(port);
        }

        string pathAndQuery = request.GetPathAndQueryProperty();
        if (string.IsNullOrWhiteSpace(pathAndQuery))
        {
            if (request.RequestUri.IsAbsoluteUri)
            {
                pathAndQuery = request.RequestUri.PathAndQuery;
            }
            else
            {
                pathAndQuery = Uri.EscapeDataString(request.RequestUri.ToString());
            }
            request.SetPathAndQueryProperty(pathAndQuery);
        }
    }

    private static void ProcessHostHeader(HttpRequestMessage request)
    {
        if (string.IsNullOrWhiteSpace(request.Headers.Host))
        {
            string host = request.GetHostProperty();
            int port = request.GetPortProperty().Value;
            if (host.Contains(':'))
            {
                // IPv6
                host = '[' + host + ']';
            }

            request.Headers.Host = host + ":" + port.ToString(CultureInfo.InvariantCulture);
        }
    }

    private ProxyMode DetermineProxyModeAndAddressLine(HttpRequestMessage request)
    {
        string scheme = request.GetSchemeProperty();
        string host = request.GetHostProperty();
        int? port = request.GetPortProperty();
        string pathAndQuery = request.GetPathAndQueryProperty();
        string addressLine = request.GetAddressLineProperty();

        if (string.IsNullOrEmpty(addressLine))
        {
            request.SetAddressLineProperty(pathAndQuery);
        }

        try
        {
            if (!UseProxy || Proxy == null || Proxy.IsBypassed(request.RequestUri))
            {
                return ProxyMode.None;
            }
        }
        catch (PlatformNotSupportedException)
        {
            return ProxyMode.None;
        }

        var proxyUri = Proxy.GetProxy(request.RequestUri);
        if (proxyUri == null)
        {
            return ProxyMode.None;
        }

        if (request.IsHttp())
        {
            if (string.IsNullOrEmpty(addressLine))
            {
                addressLine = scheme + "://" + host + ":" + port.Value + pathAndQuery;
                request.SetAddressLineProperty(addressLine);
            }
            request.SetConnectionHostProperty(proxyUri.DnsSafeHost);
            request.SetConnectionPortProperty(proxyUri.Port);
            return ProxyMode.Http;
        }

        // Tunneling generates a completely separate request, don't alter the original, just the connection address.
        request.SetConnectionHostProperty(proxyUri.DnsSafeHost);
        request.SetConnectionPortProperty(proxyUri.Port);
        return ProxyMode.Tunnel;
    }

    private async Task<Socket> TcpSocketOpenerAsync(string host, int port, CancellationToken cancellationToken)
    {
#if NET5_0_OR_GREATER
        // Use modern DNS resolution with cancellation support
        var addresses = await Dns.GetHostAddressesAsync(host, cancellationToken)
            .ConfigureAwait(false);
#else
        var addresses = await Dns.GetHostAddressesAsync(host)
            .ConfigureAwait(false);
#endif

        if (addresses.Length == 0)
        {
            throw new Exception($"Unable to resolve any IP addresses for the host '{host}'.");
        }

#if NET6_0_OR_GREATER
        // Use Happy Eyeballs if enabled and we have both IPv6 and IPv4 addresses
        if (_socketConfiguration.EnableHappyEyeballs)
        {
            var ipv6Addresses = addresses.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6).ToArray();
            var ipv4Addresses = addresses.Where(a => a.AddressFamily == AddressFamily.InterNetwork).ToArray();

            if (ipv6Addresses.Length > 0 && ipv4Addresses.Length > 0)
            {
                return await ConnectWithHappyEyeballsAsync(ipv6Addresses, ipv4Addresses, port, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
#endif

        // Fallback to sequential connection attempts
        return await ConnectSequentialAsync(addresses, port, cancellationToken).ConfigureAwait(false);
    }

    private async Task<Socket> ConnectSequentialAsync(IPAddress[] addresses, int port, CancellationToken cancellationToken)
    {
        var exceptions = new List<Exception>();

        foreach (var address in addresses)
        {
            var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Apply socket configuration for better proxy compatibility
                _socketConfiguration.ApplyTo(socket);

#if NET5_0_OR_GREATER
                // Use modern ConnectAsync with cancellation support
                await socket.ConnectAsync(address, port, cancellationToken)
                    .ConfigureAwait(false);
#else
                await socket.ConnectAsync(address, port)
                    .ConfigureAwait(false);
#endif

                return socket;
            }
            catch (Exception e)
            {
                socket.Dispose();
                exceptions.Add(e);
            }
        }

        throw new AggregateException(exceptions);
    }

#if NET6_0_OR_GREATER
    private async Task<Socket> ConnectWithHappyEyeballsAsync(
        IPAddress[] ipv6Addresses,
        IPAddress[] ipv4Addresses,
        int port,
        CancellationToken cancellationToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var exceptions = new ConcurrentBag<Exception>();
        Socket winningSocket = null;

        // Start IPv6 connection attempts
        var ipv6Task = Task.Run(async () =>
        {
            try
            {
                return await ConnectToAddressesAsync(ipv6Addresses, port, cts.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
                return null;
            }
        }, cts.Token);

        // Start IPv4 connection after delay (Happy Eyeballs delay)
        var ipv4Task = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(_socketConfiguration.HappyEyeballsDelay, cts.Token).ConfigureAwait(false);

                // Only proceed if IPv6 hasn't connected yet
                if (!ipv6Task.IsCompleted)
                {
                    return await ConnectToAddressesAsync(ipv4Addresses, port, cts.Token).ConfigureAwait(false);
                }

                return null;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
                return null;
            }
        }, cts.Token);

        // Wait for either to complete
        var completedTasks = new List<Task<Socket>> { ipv6Task, ipv4Task };

        while (completedTasks.Count > 0)
        {
            var completedTask = await Task.WhenAny(completedTasks).ConfigureAwait(false);
            completedTasks.Remove(completedTask);

            try
            {
                var socket = await completedTask.ConfigureAwait(false);
                if (socket != null && socket.Connected)
                {
                    winningSocket = socket;
                    cts.Cancel(); // Cancel the other connection attempt
                    break;
                }
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }

        // Clean up any remaining sockets from tasks that didn't win
        foreach (var task in completedTasks)
        {
            try
            {
                if (task.IsCompletedSuccessfully)
                {
                    var socket = await task.ConfigureAwait(false);
                    socket?.Dispose();
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        if (winningSocket != null)
        {
            return winningSocket;
        }

        throw new AggregateException("Failed to connect using Happy Eyeballs.", exceptions);
    }

    private async Task<Socket> ConnectToAddressesAsync(IPAddress[] addresses, int port, CancellationToken cancellationToken)
    {
        foreach (var address in addresses)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socketConfiguration.ApplyTo(socket);
                await socket.ConnectAsync(address, port, cancellationToken).ConfigureAwait(false);
                return socket;
            }
            catch (OperationCanceledException)
            {
                socket.Dispose();
                throw;
            }
            catch
            {
                socket.Dispose();
                // Try next address
            }
        }

        return null;
    }
#endif

    private async Task TunnelThroughProxyAsync(HttpRequestMessage request, Stream transport, CancellationToken cancellationToken)
    {
        // Send a Connect request:
        // CONNECT server.example.com:80 HTTP / 1.1
        // Host: server.example.com:80
        var connectRequest = new HttpRequestMessage();
        connectRequest.Version = new Version(1, 1);

        connectRequest.Headers.ProxyAuthorization = request.Headers.ProxyAuthorization;
        connectRequest.Method = new HttpMethod("CONNECT");
        // TODO: IPv6 hosts
        string authority = request.GetHostProperty() + ":" + request.GetPortProperty().Value;
        connectRequest.SetAddressLineProperty(authority);
        connectRequest.Headers.Host = authority;

        HttpConnection connection = new HttpConnection(new BufferedReadStream(transport, null, _logger));
        HttpResponseMessage connectResponse;
        try
        {
            connectResponse = await connection.SendAsync(connectRequest, cancellationToken);
            // TODO:? await connectResponse.Content.LoadIntoBufferAsync(); // Drain any body
            // There's no danger of accidentally consuming real response data because the real request hasn't been sent yet.
        }
        catch (Exception ex)
        {
            transport.Dispose();
            throw new HttpRequestException("SSL Tunnel failed to initialize", ex);
        }

        // Listen for a response. Any 2XX is considered success, anything else is considered a failure.
        if ((int)connectResponse.StatusCode < 200 || 300 <= (int)connectResponse.StatusCode)
        {
            transport.Dispose();
            throw new HttpRequestException("Failed to negotiate the proxy tunnel: " + connectResponse);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
#if NET5_0_OR_GREATER
            _connectionPool?.Dispose();
#endif
        }
        base.Dispose(disposing);
    }
}
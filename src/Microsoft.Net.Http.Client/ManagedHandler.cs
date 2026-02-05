namespace Microsoft.Net.Http.Client;

public class ManagedHandler : HttpMessageHandler
{
    private readonly ILogger _logger;

    private readonly StreamOpener _streamOpener;

    private readonly SocketOpener _socketOpener;

    private readonly SocketConnectionConfiguration _socketConfiguration;

    private IWebProxy _proxy;

    public delegate Task<Stream> StreamOpener(string host, int port, CancellationToken cancellationToken);

    public delegate Task<Socket> SocketOpener(string host, int port, CancellationToken cancellationToken);

    public ManagedHandler(ILogger logger)
        : this((SocketConnectionConfiguration)null, logger)
    {
    }

    public ManagedHandler(SocketConnectionConfiguration socketConfiguration, ILogger logger)
    {
        _logger = logger;
        _socketConfiguration = socketConfiguration ?? SocketConnectionConfiguration.Default;
        _socketOpener = TcpSocketOpenerAsync;
    }

    public ManagedHandler(StreamOpener opener, ILogger logger)
    {
        _logger = logger;
        _streamOpener = opener ?? throw new ArgumentNullException(nameof(opener));
    }

    public ManagedHandler(SocketOpener opener, ILogger logger)
    {
        _logger = logger;
        _socketOpener = opener ?? throw new ArgumentNullException(nameof(opener));
    }

    public IWebProxy Proxy
    {
        get
        {
            if (_proxy == null)
            {
#if NET5_0_OR_GREATER
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
        request.Headers.ConnectionClose = !request.Headers.Contains("Connection"); // TODO: Connection reuse is not supported.

        ProxyMode proxyMode = DetermineProxyModeAndAddressLine(request);

        if (proxyMode == ProxyMode.Http && request.Headers.ProxyAuthorization == null)
        {
            var proxyUri = Proxy.GetProxy(request.RequestUri);
            var proxyAuth = GetProxyAuthorizationHeader(proxyUri);
            if (proxyAuth != null)
            {
                request.Headers.ProxyAuthorization = proxyAuth;
            }
        }

        Socket socket;
        Stream transport;

        try
        {
            if (_socketOpener != null)
            {
                socket = await _socketOpener(request.GetConnectionHostProperty(), request.GetConnectionPortProperty().Value, cancellationToken).ConfigureAwait(false);
                transport = new NetworkStream(socket, true);
            }
            else
            {
                socket = null;
                transport = await _streamOpener(request.GetConnectionHostProperty(), request.GetConnectionPortProperty().Value, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (SocketException e)
        {
            throw new HttpRequestException("Connection failed.", e);
        }

        if (proxyMode == ProxyMode.Tunnel)
        {
            (transport, socket) = await TunnelThroughProxyAsync(request, transport, socket, cancellationToken);
        }

        if (request.IsHttps())
        {
            SslStream sslStream = new SslStream(transport, false, ServerCertificateValidationCallback);
            await sslStream.AuthenticateAsClientAsync(request.GetHostProperty(), ClientCertificates, SslProtocols.None, false);
            transport = sslStream;
        }

        var bufferedReadStream = new BufferedReadStream(transport, socket, _logger);
        var connection = new HttpConnection(bufferedReadStream);
        return await connection.SendAsync(request, cancellationToken);
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
        var addresses = await Dns.GetHostAddressesAsync(host)
            .ConfigureAwait(false);

        if (addresses.Length == 0)
        {
            throw new Exception($"Unable to resolve any IP addresses for the host '{host}'.");
        }

        var exceptions = new List<Exception>();

        foreach (var address in addresses)
        {
            var socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socketConfiguration?.Apply(socket);

                await socket.ConnectAsync(address, port)
                    .ConfigureAwait(false);

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

    internal AuthenticationHeaderValue GetProxyAuthorizationHeader(Uri proxyUri)
    {
        if (Proxy?.Credentials == null)
        {
            return null;
        }

        var credential = Proxy.Credentials.GetCredential(proxyUri, "Basic");
        if (credential == null)
        {
            return null;
        }

        var encoded = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{credential.UserName}:{credential.Password}"));
        return new AuthenticationHeaderValue("Basic", encoded);
    }

    internal async Task<(Stream transport, Socket socket)> TunnelThroughProxyAsync(HttpRequestMessage request, Stream transport, Socket socket, CancellationToken cancellationToken)
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

        // Apply proxy credentials if not already set on the original request
        if (connectRequest.Headers.ProxyAuthorization == null)
        {
            var proxyUri = Proxy.GetProxy(request.RequestUri);
            connectRequest.Headers.ProxyAuthorization = GetProxyAuthorizationHeader(proxyUri);
        }

        HttpConnection connection = new HttpConnection(new BufferedReadStream(transport, null, _logger));
        HttpResponseMessage connectResponse;
        try
        {
            connectResponse = await connection.SendAsync(connectRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            transport.Dispose();
            throw new HttpRequestException("SSL Tunnel failed to initialize", ex);
        }

        // Handle 407 Proxy Authentication Required by retrying with credentials
        if (connectResponse.StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
        {
            transport.Dispose();

            var proxyUri = Proxy.GetProxy(request.RequestUri);
            var proxyAuth = GetProxyAuthorizationHeader(proxyUri);
            if (proxyAuth == null)
            {
                throw new HttpRequestException("Proxy requires authentication but no credentials were provided.");
            }

            // Reconnect to the proxy
            if (_socketOpener != null)
            {
                socket = await _socketOpener(
                    request.GetConnectionHostProperty(),
                    request.GetConnectionPortProperty().Value,
                    cancellationToken).ConfigureAwait(false);
                transport = new NetworkStream(socket, true);
            }
            else
            {
                socket = null;
                transport = await _streamOpener(
                    request.GetConnectionHostProperty(),
                    request.GetConnectionPortProperty().Value,
                    cancellationToken).ConfigureAwait(false);
            }

            // Retry CONNECT with credentials
            var retryRequest = new HttpRequestMessage();
            retryRequest.Version = new Version(1, 1);
            retryRequest.Method = new HttpMethod("CONNECT");
            retryRequest.SetAddressLineProperty(authority);
            retryRequest.Headers.Host = authority;
            retryRequest.Headers.ProxyAuthorization = proxyAuth;

            var retryConnection = new HttpConnection(
                new BufferedReadStream(transport, socket, _logger));

            try
            {
                connectResponse = await retryConnection.SendAsync(retryRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                transport.Dispose();
                throw new HttpRequestException("SSL Tunnel failed to initialize after authentication", ex);
            }

            if ((int)connectResponse.StatusCode < 200 || 300 <= (int)connectResponse.StatusCode)
            {
                transport.Dispose();
                throw new HttpRequestException("Failed to negotiate the proxy tunnel after authentication: " + connectResponse);
            }

            return (transport, socket);
        }

        // Listen for a response. Any 2XX is considered success, anything else is considered a failure.
        if ((int)connectResponse.StatusCode < 200 || 300 <= (int)connectResponse.StatusCode)
        {
            transport.Dispose();
            throw new HttpRequestException("Failed to negotiate the proxy tunnel: " + connectResponse);
        }

        return (transport, socket);
    }
}
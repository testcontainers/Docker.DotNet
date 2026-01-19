namespace Docker.DotNet;

using System;
using System.IO.Pipes;

public sealed class DockerClient : IDockerClient
{
    internal readonly IEnumerable<ApiResponseErrorHandlingDelegate> NoErrorHandlers = Enumerable.Empty<ApiResponseErrorHandlingDelegate>();

    private const string UserAgent = "Docker.DotNet";

    private readonly HttpClient _client;

#if NET8_0_OR_GREATER
    // Separate client for hijacked stream operations (attach/exec) when using SocketsHttpHandler.
    // SocketsHttpHandler doesn't support connection hijacking, so we need ManagedHandler for those operations.
    private readonly HttpClient _hijackClient;
#endif

    private readonly Uri _endpointBaseUri;

    private readonly Version _requestedApiVersion;

    internal DockerClient(DockerClientConfiguration configuration, Version requestedApiVersion, ILogger logger = null)
    {
        _requestedApiVersion = requestedApiVersion;

        Configuration = configuration;
        DefaultTimeout = configuration.DefaultTimeout;

        Images = new ImageOperations(this);
        Containers = new ContainerOperations(this);
        System = new SystemOperations(this);
        Networks = new NetworkOperations(this);
        Secrets = new SecretsOperations(this);
        Configs = new ConfigOperations(this);
        Swarm = new SwarmOperations(this);
        Tasks = new TasksOperations(this);
        Volumes = new VolumeOperations(this);
        Plugin = new PluginOperations(this);
        Exec = new ExecOperations(this);

        HttpMessageHandler handler;
        var uri = Configuration.EndpointBaseUri;
        switch (uri.Scheme.ToLowerInvariant())
        {
            case "npipe":
                if (Configuration.Credentials.IsTlsCredentials())
                {
                    throw new Exception("TLS not supported over npipe");
                }

                var segments = uri.Segments;
                if (segments.Length != 3 || !segments[1].Equals("pipe/", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException($"{Configuration.EndpointBaseUri} is not a valid npipe URI");
                }

                var serverName = uri.Host;
                if (string.Equals(serverName, "localhost", StringComparison.OrdinalIgnoreCase))
                {
                    // npipe schemes dont work with npipe://localhost/... and need npipe://./... so fix that for a client here.
                    serverName = ".";
                }

                var pipeName = uri.Segments[2];

                uri = new UriBuilder("http", pipeName).Uri;
                handler = new ManagedHandler(async (host, port, cancellationToken) =>
                {
                    var timeout = (int)Configuration.NamedPipeConnectTimeout.TotalMilliseconds;
                    var stream = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
                    var dockerStream = new DockerPipeStream(stream);

                    await stream.ConnectAsync(timeout, cancellationToken)
                        .ConfigureAwait(false);

                    return dockerStream;
                }, logger);
                break;

            case "tcp":
            case "http":
                var builder = new UriBuilder(uri)
                {
                    Scheme = configuration.Credentials.IsTlsCredentials() ? "https" : "http"
                };
                uri = builder.Uri;
                handler = new ManagedHandler(logger);
                break;

            case "https":
                handler = new ManagedHandler(logger);
                break;

            case "unix":
                var pipeString = uri.LocalPath;
                var socketTimeout = Configuration.SocketConnectTimeout;
                uri = new UriBuilder("http", uri.Segments.Last()).Uri;
#if NET8_0_OR_GREATER
                // Use SocketsHttpHandler for regular operations (better proxy compatibility)
                handler = new SocketsHttpHandler
                {
                    ConnectCallback = async (_, cancellationToken) =>
                    {
                        var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

                        try
                        {
                            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                            timeoutCts.CancelAfter(socketTimeout);

                            await socket.ConnectAsync(new System.Net.Sockets.UnixDomainSocketEndPoint(pipeString), timeoutCts.Token)
                                .ConfigureAwait(false);

                            return new NetworkStream(socket, true);
                        }
                        catch
                        {
                            socket.Dispose();
                            throw;
                        }
                    }
                };

                // ManagedHandler is required for hijacked stream operations (attach/exec).
                // SocketsHttpHandler doesn't support the HttpConnectionResponseContent needed for connection hijacking.
                var hijackHandler = new ManagedHandler(async (_, _, cancellationToken) =>
                {
                    var endpoint = new Microsoft.Net.Http.Client.UnixDomainSocketEndPoint(pipeString);
                    var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

                    try
                    {
                        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        timeoutCts.CancelAfter(socketTimeout);

                        var connectTask = socket.ConnectAsync(endpoint);
                        var timeoutTask = Task.Delay(Timeout.InfiniteTimeSpan, timeoutCts.Token);

                        var completedTask = await Task.WhenAny(connectTask, timeoutTask)
                            .ConfigureAwait(false);

                        if (completedTask == timeoutTask)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            throw new TimeoutException($"Connection to Unix socket '{pipeString}' timed out after {socketTimeout.TotalSeconds}s.");
                        }

                        await connectTask.ConfigureAwait(false);
                        return socket;
                    }
                    catch
                    {
                        socket.Dispose();
                        throw;
                    }
                }, logger);

                _endpointBaseUri = uri;
                _client = new HttpClient(Configuration.Credentials.GetHandler(handler), true);
                _client.Timeout = Timeout.InfiniteTimeSpan;
                _hijackClient = new HttpClient(Configuration.Credentials.GetHandler(hijackHandler), true);
                _hijackClient.Timeout = Timeout.InfiniteTimeSpan;
                return;
#else
                handler = new ManagedHandler(async (_, _, cancellationToken) =>
                {
                    var endpoint = new Microsoft.Net.Http.Client.UnixDomainSocketEndPoint(pipeString);
                    var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

                    try
                    {
                        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        timeoutCts.CancelAfter(socketTimeout);

                        var connectTask = socket.ConnectAsync(endpoint);
                        var timeoutTask = Task.Delay(Timeout.InfiniteTimeSpan, timeoutCts.Token);

                        var completedTask = await Task.WhenAny(connectTask, timeoutTask)
                            .ConfigureAwait(false);

                        if (completedTask == timeoutTask)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            throw new TimeoutException($"Connection to Unix socket '{pipeString}' timed out after {socketTimeout.TotalSeconds}s.");
                        }

                        await connectTask.ConfigureAwait(false);
                        return socket;
                    }
                    catch
                    {
                        socket.Dispose();
                        throw;
                    }
                }, logger);
                break;
#endif

            default:
                throw new Exception($"Unknown URL scheme {configuration.EndpointBaseUri.Scheme}");
        }

        _endpointBaseUri = uri;

        _client = new HttpClient(Configuration.Credentials.GetHandler(handler), true);
        _client.Timeout = Timeout.InfiniteTimeSpan;
    }

    public DockerClientConfiguration Configuration { get; }

    public TimeSpan DefaultTimeout { get; set; }

    public IContainerOperations Containers { get; }

    public IImageOperations Images { get; }

    public INetworkOperations Networks { get; }

    public IVolumeOperations Volumes { get; }

    public ISecretsOperations Secrets { get; }

    public IConfigOperations Configs { get; }

    public ISwarmOperations Swarm { get; }

    public ITasksOperations Tasks { get; }

    public ISystemOperations System { get; }

    public IPluginOperations Plugin { get; }

    public IExecOperations Exec { get; }

    internal static JsonSerializer JsonSerializer => JsonSerializer.Instance;

    public void Dispose()
    {
        Configuration.Dispose();
        _client.Dispose();
#if NET8_0_OR_GREATER
        _hijackClient?.Dispose();
#endif
    }

    internal Task MakeRequestAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        CancellationToken token)
    {
        return MakeRequestAsync<NoContent>(errorHandlers, method, path, null, null, token);
    }

    internal Task<T> MakeRequestAsync<T>(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        CancellationToken token)
    {
        return MakeRequestAsync<T>(errorHandlers, method, path, null, null, token);
    }

    internal Task MakeRequestAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        CancellationToken token)
    {
        return MakeRequestAsync<NoContent>(errorHandlers, method, path, queryString, null, token);
    }

    internal Task<T> MakeRequestAsync<T>(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        CancellationToken token)
    {
        return MakeRequestAsync<T>(errorHandlers, method, path, queryString, null, token);
    }

    internal Task MakeRequestAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        CancellationToken token)
    {
        return MakeRequestAsync<NoContent>(errorHandlers, method, path, queryString, body, null, token);
    }

    internal Task<T> MakeRequestAsync<T>(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        CancellationToken token)
    {
        return MakeRequestAsync<T>(errorHandlers, method, path, queryString, body, null, token);
    }

    internal Task<T> MakeRequestAsync<T>(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        CancellationToken token)
    {
        return MakeRequestAsync<T>(errorHandlers, method, path, queryString, body, headers, DefaultTimeout, token);
    }

    internal Task MakeRequestAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        TimeSpan timeout,
        CancellationToken token)
    {
        return MakeRequestAsync<NoContent>(errorHandlers, method, path, queryString, body, headers, timeout, token);
    }

    internal async Task<T> MakeRequestAsync<T>(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        TimeSpan timeout,
        CancellationToken token)
    {
        using var response = await PrivateMakeRequestAsync(timeout, HttpCompletionOption.ResponseContentRead, method, path, queryString, headers, body, token)
            .ConfigureAwait(false);

        await HandleIfErrorResponseAsync(response.StatusCode, response, errorHandlers)
            .ConfigureAwait(false);

        if (typeof(T) == typeof(NoContent))
        {
            return default;
        }

        return await JsonSerializer.DeserializeAsync<T>(response.Content, token)
            .ConfigureAwait(false);
    }

    internal Task<Stream> MakeRequestForStreamAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        CancellationToken token)
    {
        return MakeRequestForStreamAsync(errorHandlers, method, path, null, token);
    }

    internal Task<Stream> MakeRequestForStreamAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        CancellationToken token)
    {
        return MakeRequestForStreamAsync(errorHandlers, method, path, queryString, null, token);
    }

    internal Task<Stream> MakeRequestForStreamAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        CancellationToken token)
    {
        return MakeRequestForStreamAsync(errorHandlers, method, path, queryString, body, null, token);
    }

    internal Task<Stream> MakeRequestForStreamAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        CancellationToken token)
    {
        return MakeRequestForStreamAsync(errorHandlers, method, path, queryString, body, headers, Timeout.InfiniteTimeSpan, token);
    }

    internal async Task<Stream> MakeRequestForStreamAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        TimeSpan timeout,
        CancellationToken token)
    {
        var response = await PrivateMakeRequestAsync(timeout, HttpCompletionOption.ResponseHeadersRead, method, path, queryString, headers, body, token)
            .ConfigureAwait(false);

        await HandleIfErrorResponseAsync(response.StatusCode, response, errorHandlers)
            .ConfigureAwait(false);

        return await response.Content.ReadAsStreamAsync()
            .ConfigureAwait(false);
    }

    internal async Task<HttpResponseMessage> MakeRequestForRawResponseAsync(
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        CancellationToken token)
    {
        var response = await PrivateMakeRequestAsync(Timeout.InfiniteTimeSpan, HttpCompletionOption.ResponseHeadersRead, method, path, queryString, headers, body, token)
            .ConfigureAwait(false);

        await HandleIfErrorResponseAsync(response.StatusCode, response)
            .ConfigureAwait(false);

        return response;
    }

    internal async Task<DockerApiStreamedResponse> MakeRequestForStreamedResponseAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        CancellationToken cancellationToken)
    {
        var response = await PrivateMakeRequestAsync(Timeout.InfiniteTimeSpan, HttpCompletionOption.ResponseHeadersRead, method, path, queryString, null, null, cancellationToken)
            .ConfigureAwait(false);

        await HandleIfErrorResponseAsync(response.StatusCode, response, errorHandlers)
            .ConfigureAwait(false);

        var body = await response.Content.ReadAsStreamAsync()
            .ConfigureAwait(false);

        return new DockerApiStreamedResponse(response.StatusCode, body, response.Headers);
    }

    internal Task<WriteClosableStream> MakeRequestForHijackedStreamAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        CancellationToken cancellationToken)
    {
        return MakeRequestForHijackedStreamAsync(errorHandlers, method, path, queryString, body, headers, Timeout.InfiniteTimeSpan, cancellationToken);
    }

    internal async Task<WriteClosableStream> MakeRequestForHijackedStreamAsync(
        IEnumerable<ApiResponseErrorHandlingDelegate> errorHandlers,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IRequestContent body,
        IDictionary<string, string> headers,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        // The Docker Engine API docs sounds like these headers are optional, but if they
        // aren't include in the request, the daemon doesn't set up the raw stream
        // correctly. Either the headers are always required, or they're necessary
        // specifically in Docker Desktop environments because of some internal communication
        // (using a proxy).

        if (headers == null)
        {
            headers = new Dictionary<string, string>();
        }

        headers.Add("Upgrade", "tcp");
        headers.Add("Connection", "upgrade");

#if NET8_0_OR_GREATER
        // Use _hijackClient for SocketsHttpHandler scenarios where _client doesn't support hijacking
        var response = await PrivateMakeRequestAsync(_hijackClient ?? _client, timeout, HttpCompletionOption.ResponseHeadersRead, method, path, queryString, headers, body, cancellationToken)
            .ConfigureAwait(false);
#else
        var response = await PrivateMakeRequestAsync(timeout, HttpCompletionOption.ResponseHeadersRead, method, path, queryString, headers, body, cancellationToken)
            .ConfigureAwait(false);
#endif

        await HandleIfErrorResponseAsync(response.StatusCode, response, errorHandlers)
            .ConfigureAwait(false);

        if (response.Content is not HttpConnectionResponseContent content)
        {
            throw new NotSupportedException("message handler does not support hijacked streams");
        }

        return content.HijackStream();
    }

    private Task<HttpResponseMessage> PrivateMakeRequestAsync(
        TimeSpan timeout,
        HttpCompletionOption completionOption,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IDictionary<string, string> headers,
        IRequestContent data,
        CancellationToken cancellationToken)
    {
        return PrivateMakeRequestAsync(_client, timeout, completionOption, method, path, queryString, headers, data, cancellationToken);
    }

    private async Task<HttpResponseMessage> PrivateMakeRequestAsync(
        HttpClient client,
        TimeSpan timeout,
        HttpCompletionOption completionOption,
        HttpMethod method,
        string path,
        IQueryString queryString,
        IDictionary<string, string> headers,
        IRequestContent data,
        CancellationToken cancellationToken)
    {
        using var request = PrepareRequest(method, path, queryString, headers, data);

        if (Timeout.InfiniteTimeSpan == timeout)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            using var disposable = cancellationToken.Register(() => tcs.SetCanceled());

            return await await Task.WhenAny(tcs.Task, client.SendAsync(request, completionOption, cancellationToken))
                .ConfigureAwait(false);
        }
        else
        {
            using var timeoutCts = new CancellationTokenSource(timeout);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);

            return await client.SendAsync(request, completionOption, linkedCts.Token)
                .ConfigureAwait(false);
        }
    }

    private HttpRequestMessage PrepareRequest(HttpMethod method, string path, IQueryString queryString, IDictionary<string, string> headers, IRequestContent data)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentNullException(nameof(path));
        }

        var request = new HttpRequestMessage(method, HttpUtility.BuildUri(_endpointBaseUri, _requestedApiVersion, path, queryString));
        request.Version = new Version(1, 1);
        request.Headers.Add("User-Agent", UserAgent);

        var customHeaders = headers == null
            ? Configuration.DefaultHttpRequestHeaders
            : Configuration.DefaultHttpRequestHeaders.Concat(headers);

        foreach (var header in customHeaders)
        {
            request.Headers.Add(header.Key, header.Value);
        }

        if (data != null)
        {
            var requestContent = data.GetContent(); // make the call only once.
            request.Content = requestContent;
        }

        return request;
    }

    private async Task HandleIfErrorResponseAsync(HttpStatusCode statusCode, HttpResponseMessage response, IEnumerable<ApiResponseErrorHandlingDelegate> handlers)
    {
        var isErrorResponse = (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest) && statusCode != HttpStatusCode.SwitchingProtocols;

        string responseBody = null;

        if (isErrorResponse)
        {
            // If it is not an error response, we do not read the response body because the caller may wish to consume it.
            // If it is an error response, we do because there is nothing else going to be done with it anyway and
            // we want to report the response body in the error message as it contains potentially useful info.
            responseBody = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
        }

        // If no customer handlers just default the response.
        if (handlers != null)
        {
            foreach (var handler in handlers)
            {
                handler(statusCode, responseBody);
            }
        }

        // No custom handler was fired. Default the response for generic success/failures.
        if (isErrorResponse)
        {
            throw new DockerApiException(statusCode, responseBody);
        }
    }

    private async Task HandleIfErrorResponseAsync(HttpStatusCode statusCode, HttpResponseMessage response)
    {
        var isErrorResponse = statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest;

        string responseBody = null;

        if (isErrorResponse)
        {
            // If it is not an error response, we do not read the response body because the caller may wish to consume it.
            // If it is an error response, we do because there is nothing else going to be done with it anyway and
            // we want to report the response body in the error message as it contains potentially useful info.
            responseBody = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
        }

        // No custom handler was fired. Default the response for generic success/failures.
        if (isErrorResponse)
        {
            throw new DockerApiException(statusCode, responseBody);
        }
    }

    private struct NoContent;
}

internal delegate void ApiResponseErrorHandlingDelegate(HttpStatusCode statusCode, string responseBody);
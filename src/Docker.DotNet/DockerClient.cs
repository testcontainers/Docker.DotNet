namespace Docker.DotNet;

using System;

public sealed class DockerClient : IDockerClient
{
    internal readonly IEnumerable<ApiResponseErrorHandlingDelegate> NoErrorHandlers = Enumerable.Empty<ApiResponseErrorHandlingDelegate>();

    private const string UserAgent = "Docker.DotNet";

    private readonly HttpClient _client;

    private readonly Uri _endpointBaseUri;

    private readonly Version _requestedApiVersion;

    private readonly IDockerHandlerFactory _handlerFactory;

    internal DockerClient(DockerClientConfiguration configuration, Version requestedApiVersion, IDockerHandlerFactory handlerFactory, ILogger logger = null)
    {
        if (handlerFactory == null)
        {
            throw new ArgumentNullException(nameof(handlerFactory));
        }

        _requestedApiVersion = requestedApiVersion;
        _handlerFactory = handlerFactory;

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

        var (handler, endpoint) = _handlerFactory.CreateHandler(Configuration.EndpointBaseUri, Configuration, logger);

        _client = new HttpClient(Configuration.Credentials.GetHandler(handler), true);
        _client.Timeout = Timeout.InfiniteTimeSpan;
        _endpointBaseUri = endpoint;
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

        var response = await PrivateMakeRequestAsync(timeout, HttpCompletionOption.ResponseHeadersRead, method, path, queryString, headers, body, cancellationToken)
            .ConfigureAwait(false);

        await HandleIfErrorResponseAsync(response.StatusCode, response, errorHandlers)
            .ConfigureAwait(false);

        return await _handlerFactory.HijackStreamAsync(response.Content)
            .ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> PrivateMakeRequestAsync(
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

            return await await Task.WhenAny(tcs.Task, _client.SendAsync(request, completionOption, cancellationToken))
                .ConfigureAwait(false);
        }
        else
        {
            using var timeoutCts = new CancellationTokenSource(timeout);

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);

            return await _client.SendAsync(request, completionOption, linkedCts.Token)
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
            // If it is an error response, we do because there is nothing else going to be done with it anyway, and
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
            // If it is an error response, we do because there is nothing else going to be done with it anyway, and
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
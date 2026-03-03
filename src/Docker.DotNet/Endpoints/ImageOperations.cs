namespace Docker.DotNet;

internal class ImageOperations : IImageOperations
{
    private const string RegistryAuthHeaderKey = "X-Registry-Auth";

    private const string RegistryConfigHeaderKey = "X-Registry-Config";

    private const string TarContentType = "application/x-tar";

    private const string ImportFromBodySource = "-";

    private static readonly ApiResponseErrorHandlingDelegate NoSuchImageHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.NotFound)
        {
            throw new DockerImageNotFoundException(statusCode, responseBody);
        }
    };

    private readonly DockerClient _client;

    internal ImageOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<IList<ImagesListResponse>> ListImagesAsync(ImagesListParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ImagesListParameters>(parameters);
        return await _client.MakeRequestAsync<ImagesListResponse[]>(_client.NoErrorHandlers, HttpMethod.Get, "images/json", queryParameters, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Stream> BuildImageFromDockerfileAsync(Stream contents, ImageBuildParameters parameters, CancellationToken cancellationToken = default)
    {
        if (contents == null)
        {
            throw new ArgumentNullException(nameof(contents));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ImageBuildParameters>(parameters);

        var data = new BinaryRequestContent(contents, TarContentType);

        return await _client.MakeRequestForStreamAsync(_client.NoErrorHandlers, HttpMethod.Post, "build", queryParameters, data, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task BuildImageFromDockerfileAsync(ImageBuildParameters parameters, Stream contents, IEnumerable<AuthConfig>? authConfigs, IDictionary<string, string>? headers, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        if (contents == null)
        {
            throw new ArgumentNullException(nameof(contents));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        HttpMethod httpMethod = HttpMethod.Post;

        var data = new BinaryRequestContent(contents, TarContentType);

        var queryParameters = new QueryString<ImageBuildParameters>(parameters);

        Dictionary<string, string> customHeaders = RegistryConfigHeaders(authConfigs);

        if (headers != null)
        {
            foreach (string key in headers.Keys)
            {
                customHeaders[key] = headers[key];
            }
        }

        return StreamUtil.MonitorResponseForMessagesAsync(
            _client.MakeRequestForRawResponseAsync(httpMethod, "build", queryParameters, data, customHeaders, cancellationToken),
            progress,
            cancellationToken);
    }

    public Task CreateImageAsync(ImagesCreateParameters parameters, AuthConfig authConfig, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        return CreateImageAsync(parameters, null, authConfig, null, progress, cancellationToken);
    }

    public Task CreateImageAsync(ImagesCreateParameters parameters, AuthConfig authConfig, IDictionary<string, string>? headers, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        return CreateImageAsync(parameters, null, authConfig, headers, progress, cancellationToken);
    }

    public Task CreateImageAsync(ImagesCreateParameters parameters, Stream imageStream, AuthConfig authConfig, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        return CreateImageAsync(parameters, imageStream, authConfig, null, progress, cancellationToken);
    }

    public Task CreateImageAsync(ImagesCreateParameters parameters, Stream? imageStream, AuthConfig authConfig, IDictionary<string, string>? headers, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        HttpMethod httpMethod = HttpMethod.Post;
        BinaryRequestContent? content = null;
        if (imageStream != null)
        {
            content = new BinaryRequestContent(imageStream, TarContentType);
            parameters.FromSrc = ImportFromBodySource;
        }

        var queryParameters = new QueryString<ImagesCreateParameters>(parameters);

        Dictionary<string, string> customHeaders = RegistryAuthHeaders(authConfig);

        if (headers != null)
        {
            foreach (var key in headers.Keys)
            {
                customHeaders[key] = headers[key];
            }
        }

        return StreamUtil.MonitorResponseForMessagesAsync(
            _client.MakeRequestForRawResponseAsync(httpMethod, "images/create", queryParameters, content, customHeaders, cancellationToken),
            progress,
            cancellationToken);
    }

    public async Task<ImageInspectResponse> InspectImageAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return await _client.MakeRequestAsync<ImageInspectResponse>([NoSuchImageHandler], HttpMethod.Get, $"images/{name}/json", cancellationToken).ConfigureAwait(false);
    }

    public async Task<IList<ImageHistoryResponse>> GetImageHistoryAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return await _client.MakeRequestAsync<ImageHistoryResponse[]>([NoSuchImageHandler], HttpMethod.Get, $"images/{name}/history", cancellationToken).ConfigureAwait(false);
    }

    public Task PushImageAsync(string name, ImagePushParameters parameters, AuthConfig authConfig, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ImagePushParameters>(parameters);

        return StreamUtil.MonitorStreamForMessagesAsync(
            _client.MakeRequestForStreamAsync(_client.NoErrorHandlers, HttpMethod.Post, $"images/{name}/push", queryParameters, null, RegistryAuthHeaders(authConfig), cancellationToken),
            progress,
            cancellationToken);
    }

    public Task TagImageAsync(string name, ImageTagParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ImageTagParameters>(parameters);
        return _client.MakeRequestAsync([NoSuchImageHandler], HttpMethod.Post, $"images/{name}/tag", queryParameters, cancellationToken);
    }

    public async Task<IList<IDictionary<string, string>>> DeleteImageAsync(string name, ImageDeleteParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ImageDeleteParameters>(parameters);
        return await _client.MakeRequestAsync<Dictionary<string, string>[]>([NoSuchImageHandler], HttpMethod.Delete, $"images/{name}", queryParameters, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IList<ImageSearchResponse>> SearchImagesAsync(ImagesSearchParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ImagesSearchParameters>(parameters);
        return await _client.MakeRequestAsync<ImageSearchResponse[]>(_client.NoErrorHandlers, HttpMethod.Get, "images/search", queryParameters, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ImagesPruneResponse> PruneImagesAsync(ImagesPruneParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<ImagesPruneParameters>(parameters);
        return await _client.MakeRequestAsync<ImagesPruneResponse>(_client.NoErrorHandlers, HttpMethod.Post, "images/prune", queryParameters, cancellationToken).ConfigureAwait(false);
    }

    public async Task<CommitContainerChangesResponse> CommitContainerChangesAsync(CommitContainerChangesParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var data = new JsonRequestContent<CommitContainerChangesParameters>(parameters, DockerClient.JsonSerializer);

        var queryParameters = new QueryString<CommitContainerChangesParameters>(parameters);
        return await _client.MakeRequestAsync<CommitContainerChangesResponse>(_client.NoErrorHandlers, HttpMethod.Post, "commit", queryParameters, data, cancellationToken).ConfigureAwait(false);
    }

    public Task<Stream> SaveImageAsync(string name, CancellationToken cancellationToken = default)
    {
        return SaveImagesAsync([name], cancellationToken);
    }

    public async Task<Stream> SaveImagesAsync(string[] names, CancellationToken cancellationToken = default)
    {
        EnumerableQueryString? queryString = null;

        if (names?.Length > 0)
        {
            queryString = new EnumerableQueryString("names", names);
        }

        return await _client.MakeRequestForStreamAsync([NoSuchImageHandler], HttpMethod.Get, "images/get", queryString, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task LoadImageAsync(ImageLoadParameters parameters, Stream imageStream, IProgress<JSONMessage> progress, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (imageStream == null)
        {
            throw new ArgumentNullException(nameof(imageStream));
        }

        BinaryRequestContent content = new BinaryRequestContent(imageStream, TarContentType);

        var queryParameters = new QueryString<ImageLoadParameters>(parameters);

        return StreamUtil.MonitorStreamForMessagesAsync(
            _client.MakeRequestForStreamAsync(_client.NoErrorHandlers, HttpMethod.Post, "images/load", queryParameters, content, cancellationToken),
            progress,
            cancellationToken);
    }

    private static Dictionary<string, string> RegistryAuthHeaders(AuthConfig authConfig)
    {
        return new Dictionary<string, string>
        {
            {
                RegistryAuthHeaderKey,
                Convert.ToBase64String(DockerClient.JsonSerializer.SerializeToUtf8Bytes(authConfig ?? new AuthConfig()))
                    .Replace("/", "_").Replace("+", "-")
                // This is not documented in Docker API but from source code (https://github.com/docker/docker-ce/blob/10e40bd1548f69354a803a15fde1b672cc024b91/components/cli/cli/command/registry.go#L47)
                // and from multiple internet sources it has to be base64-url-safe.
                // See RFC 4648 Section 5. Padding (=) needs to be kept.
            }
        };
    }

    private static Dictionary<string, string> RegistryConfigHeaders(IEnumerable<AuthConfig>? authConfig)
    {
        var configDictionary = (authConfig ?? Array.Empty<AuthConfig>()).ToDictionary(e => e.ServerAddress, e => e);
        return new Dictionary<string, string>
        {
            {
                RegistryConfigHeaderKey,
                Convert.ToBase64String(DockerClient.JsonSerializer.SerializeToUtf8Bytes(configDictionary))
            }
        };
    }
}
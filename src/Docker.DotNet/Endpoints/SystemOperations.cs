namespace Docker.DotNet;

internal class SystemOperations : ISystemOperations
{
    private readonly DockerClient _client;

    internal SystemOperations(DockerClient client)
    {
        _client = client;
    }

    public Task AuthenticateAsync(AuthConfig authConfig, CancellationToken cancellationToken = default)
    {
        if (authConfig == null)
        {
            throw new ArgumentNullException(nameof(authConfig));
        }

        var data = new JsonRequestContent<AuthConfig>(authConfig, DockerClient.JsonSerializer);

        return _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Post, "auth", null, data, cancellationToken);
    }

    public async Task<VersionResponse> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        return await _client.MakeRequestAsync<VersionResponse>(_client.NoErrorHandlers, HttpMethod.Get, "version", cancellationToken).ConfigureAwait(false);
    }

    public Task PingAsync(CancellationToken cancellationToken = default)
    {
        return _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Get, "_ping", cancellationToken);
    }

    public async Task<SystemInfoResponse> GetSystemInfoAsync(CancellationToken cancellationToken = default)
    {
        return await _client.MakeRequestAsync<SystemInfoResponse>(_client.NoErrorHandlers, HttpMethod.Get, "info", cancellationToken).ConfigureAwait(false);
    }

    public async Task<Stream> MonitorEventsAsync(ContainerEventsParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString queryParameters = new QueryString<ContainerEventsParameters>(parameters);

        return await _client.MakeRequestForStreamAsync(_client.NoErrorHandlers, HttpMethod.Get, "events", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task MonitorEventsAsync(ContainerEventsParameters parameters, IProgress<Message> progress, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (progress == null)
        {
            throw new ArgumentNullException(nameof(progress));
        }

        IQueryString queryParameters = new QueryString<ContainerEventsParameters>(parameters);

        return StreamUtil.MonitorStreamForMessagesAsync(
            _client.MakeRequestForStreamAsync(_client.NoErrorHandlers, HttpMethod.Get, "events", queryParameters, cancellationToken),
            progress,
            cancellationToken);
    }
}
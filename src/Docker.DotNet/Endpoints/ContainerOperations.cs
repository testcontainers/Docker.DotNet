namespace Docker.DotNet;

internal class ContainerOperations : IContainerOperations
{
    private static readonly ApiResponseErrorHandlingDelegate NoSuchContainerHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.NotFound)
        {
            throw new DockerContainerNotFoundException(statusCode, responseBody);
        }
    };

    private static readonly ApiResponseErrorHandlingDelegate NoSuchImageHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.NotFound)
        {
            throw new DockerImageNotFoundException(statusCode, responseBody);
        }
    };

    private readonly DockerClient _client;

    internal ContainerOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<IList<ContainerListResponse>> ListContainersAsync(ContainersListParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainersListParameters>(parameters);

        return await _client.MakeRequestAsync<ContainerListResponse[]>(_client.NoErrorHandlers, HttpMethod.Get, "containers/json", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<CreateContainerResponse> CreateContainerAsync(CreateContainerParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString? queryParameters = null;
        if (!string.IsNullOrEmpty(parameters.Name))
        {
            queryParameters = new QueryString<CreateContainerParameters>(parameters);
        }

        var data = new JsonRequestContent<CreateContainerParameters>(parameters, DockerClient.JsonSerializer);

        return await _client.MakeRequestAsync<CreateContainerResponse>([NoSuchImageHandler], HttpMethod.Post, "containers/create", queryParameters, data, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ContainerInspectResponse> InspectContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<ContainerInspectResponse>([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/json", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ContainerInspectResponse> InspectContainerAsync(string id, ContainerInspectParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerInspectParameters>(parameters);

        return await _client.MakeRequestAsync<ContainerInspectResponse>([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/json", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ContainerProcessesResponse> ListProcessesAsync(string id, ContainerListProcessesParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerListProcessesParameters>(parameters);

        return await _client.MakeRequestAsync<ContainerProcessesResponse>([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/top", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task GetContainerLogsAsync(string id, ContainerLogsParameters parameters, IProgress<string> progress, CancellationToken cancellationToken = default)
    {
        using var multiplexedStream = await GetContainerLogsAsync(id, parameters, cancellationToken)
            .ConfigureAwait(false);

        await StreamUtil.MonitorStreamAsync(multiplexedStream, progress, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<MultiplexedStream> GetContainerLogsAsync(string id, ContainerLogsParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerLogsParameters>(parameters);

        var containerInspectResponse = await InspectContainerAsync(id, cancellationToken)
            .ConfigureAwait(false);

        var response = await _client.MakeRequestForStreamAsync([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/logs", queryParameters, null, null, cancellationToken)
            .ConfigureAwait(false);

        return new MultiplexedStream(response, !containerInspectResponse.Config.Tty);
    }

    public async Task<IList<ContainerFileSystemChangeResponse>> InspectChangesAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<ContainerFileSystemChangeResponse[]>([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/changes", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Stream> ExportContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestForStreamAsync([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/export", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Stream> GetContainerStatsAsync(string id, ContainerStatsParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerStatsParameters>(parameters);

        return await _client.MakeRequestForStreamAsync([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/stats", queryParameters, null, null, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task GetContainerStatsAsync(string id, ContainerStatsParameters parameters, IProgress<ContainerStatsResponse> progress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (progress == null)
        {
            throw new ArgumentNullException(nameof(progress));
        }

        var queryParameters = new QueryString<ContainerStatsParameters>(parameters);

        return StreamUtil.MonitorStreamForMessagesAsync(
            _client.MakeRequestForStreamAsync([NoSuchContainerHandler], HttpMethod.Get, $"containers/{id}/stats", queryParameters, null, null, cancellationToken),
            progress,
            cancellationToken);
    }

    public async Task ResizeContainerTtyAsync(string id, ContainerResizeParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerResizeParameters>(parameters);

        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/resize", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> StartContainerAsync(string id, ContainerStartParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        var queryParameters = parameters == null ? null : new QueryString<ContainerStartParameters>(parameters);

        bool? result = null;

        await _client.MakeRequestAsync([NoSuchContainerHandler, (statusCode, _) => result = statusCode != HttpStatusCode.NotModified], HttpMethod.Post, $"containers/{id}/start", queryParameters, cancellationToken)
            .ConfigureAwait(false);

        return result ?? throw new InvalidOperationException();
    }

    public async Task<bool> StopContainerAsync(string id, ContainerStopParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerStopParameters>(parameters);

        bool? result = null;

        // since specified wait timespan can be greater than HttpClient's default, we set the
        // client timeout to infinite and provide a cancellation token.
        await _client.MakeRequestAsync([NoSuchContainerHandler, (statusCode, _) => result = statusCode != HttpStatusCode.NotModified], HttpMethod.Post, $"containers/{id}/stop", queryParameters, null, null, Timeout.InfiniteTimeSpan, cancellationToken)
            .ConfigureAwait(false);

        return result ?? throw new InvalidOperationException();
    }

    public async Task RestartContainerAsync(string id, ContainerRestartParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerRestartParameters>(parameters);

        // since specified wait timespan can be greater than HttpClient's default, we set the
        // client timeout to infinite and provide a cancellation token.
        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/restart", queryParameters, null, null, Timeout.InfiniteTimeSpan, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task KillContainerAsync(string id, ContainerKillParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerKillParameters>(parameters);

        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/kill", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task RenameContainerAsync(string id, ContainerRenameParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        var queryParameters = new QueryString<ContainerRenameParameters>(parameters);

        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/rename", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task PauseContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/pause", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UnpauseContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/unpause", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<MultiplexedStream> AttachContainerAsync(string id, ContainerAttachParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerAttachParameters>(parameters);

        var containerInspectResponse = await InspectContainerAsync(id, cancellationToken)
            .ConfigureAwait(false);

        var response = await _client.MakeRequestForHijackedStreamAsync([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/attach", queryParameters, null, null, cancellationToken)
            .ConfigureAwait(false);

        return new MultiplexedStream(response, !containerInspectResponse.Config.Tty);
    }

    public async Task<ContainerWaitResponse> WaitContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<ContainerWaitResponse>([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/wait", null, null, null, Timeout.InfiniteTimeSpan, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task RemoveContainerAsync(string id, ContainerRemoveParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ContainerRemoveParameters>(parameters);

        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Delete, $"containers/{id}", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ContainerArchiveResponse> GetArchiveFromContainerAsync(string id, ContainerPathStatParameters parameters, bool statOnly, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        Stream? stream;

        var queryParameters = new QueryString<ContainerPathStatParameters>(parameters);

        var response = await _client.MakeRequestForStreamedResponseAsync([NoSuchContainerHandler], statOnly ? HttpMethod.Head : HttpMethod.Get, $"containers/{id}/archive", queryParameters, cancellationToken);

        var statHeader = response.Headers.GetValues("X-Docker-Container-Path-Stat").First();

        var bytes = Convert.FromBase64String(statHeader);

        var stat = DockerClient.JsonSerializer.Deserialize<ContainerPathStatResponse>(bytes);

        if (statOnly)
        {
            response.Dispose();
            stream = null;
        }
        else
        {
            stream = response;
        }

        return new ContainerArchiveResponse
        {
            Stat = stat,
            Stream = stream
        };
    }

    public async Task ExtractArchiveToContainerAsync(string id, CopyToContainerParameters parameters, Stream stream, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<CopyToContainerParameters>(parameters);

        var data = new BinaryRequestContent(stream, "application/x-tar");

        await _client.MakeRequestAsync([NoSuchContainerHandler], HttpMethod.Put, $"containers/{id}/archive", queryParameters, data, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ContainersPruneResponse> PruneContainersAsync(ContainersPruneParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<ContainersPruneParameters>(parameters);

        return await _client.MakeRequestAsync<ContainersPruneResponse>(_client.NoErrorHandlers, HttpMethod.Post, "containers/prune", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ContainerUpdateResponse> UpdateContainerAsync(string id, ContainerUpdateParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var data = new JsonRequestContent<ContainerUpdateParameters>(parameters, DockerClient.JsonSerializer);

        return await _client.MakeRequestAsync<ContainerUpdateResponse>([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/update", null, data, cancellationToken)
            .ConfigureAwait(false);
    }
}
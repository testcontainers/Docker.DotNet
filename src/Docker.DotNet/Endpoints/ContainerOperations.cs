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

        IQueryString queryParameters = new QueryString<ContainersListParameters>(parameters);
        return await _client.MakeRequestAsync<ContainerListResponse[]>(_client.NoErrorHandlers, HttpMethod.Get, "containers/json", queryParameters, cancellationToken).ConfigureAwait(false);
    }

    public async Task<CreateContainerResponse> CreateContainerAsync(CreateContainerParameters parameters, CancellationToken cancellationToken = default)
    {
        IQueryString qs = null;

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        if (!string.IsNullOrEmpty(parameters.Name))
        {
            qs = new QueryString<CreateContainerParameters>(parameters);
        }

        var data = new JsonRequestContent<CreateContainerParameters>(parameters, DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<CreateContainerResponse>(new[] { NoSuchImageHandler }, HttpMethod.Post, "containers/create", qs, data, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ContainerInspectResponse> InspectContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<ContainerInspectResponse>(new[] { NoSuchContainerHandler }, HttpMethod.Get, $"containers/{id}/json", cancellationToken).ConfigureAwait(false);
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

        IQueryString queryString = new QueryString<ContainerInspectParameters>(parameters);
        return await _client.MakeRequestAsync<ContainerInspectResponse>(new[] { NoSuchContainerHandler }, HttpMethod.Get, $"containers/{id}/json", queryString, cancellationToken).ConfigureAwait(false);
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

        IQueryString queryParameters = new QueryString<ContainerListProcessesParameters>(parameters);
        return await _client.MakeRequestAsync<ContainerProcessesResponse>(new[] { NoSuchContainerHandler }, HttpMethod.Get, $"containers/{id}/top", queryParameters, cancellationToken).ConfigureAwait(false);
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

        var stream = await _client.MakeRequestForStreamAsync(new[] { NoSuchContainerHandler }, HttpMethod.Get, $"containers/{id}/logs", queryParameters, null, null, cancellationToken)
            .ConfigureAwait(false);

        return new MultiplexedStream(stream, !containerInspectResponse.Config.Tty);
    }

    public async Task<IList<ContainerFileSystemChangeResponse>> InspectChangesAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<ContainerFileSystemChangeResponse[]>(new[] { NoSuchContainerHandler }, HttpMethod.Get, $"containers/{id}/changes", cancellationToken).ConfigureAwait(false);
    }

    public Task<Stream> ExportContainerAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return _client.MakeRequestForStreamAsync(new[] { NoSuchContainerHandler }, HttpMethod.Get, $"containers/{id}/export", cancellationToken);
    }

    public Task<Stream> GetContainerStatsAsync(string id, ContainerStatsParameters parameters, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString queryParameters = new QueryString<ContainerStatsParameters>(parameters);
        return _client.MakeRequestForStreamAsync(new[] { NoSuchContainerHandler }, HttpMethod.Get, $"containers/{id}/stats", queryParameters, null, null, cancellationToken);
    }

    public Task GetContainerStatsAsync(string id, ContainerStatsParameters parameters, IProgress<ContainerStatsResponse> progress, CancellationToken cancellationToken = default)
    {
        return StreamUtil.MonitorStreamForMessagesAsync(
            GetContainerStatsAsync(id, parameters, cancellationToken),
            _client,
            cancellationToken,
            progress);
    }

    public Task ResizeContainerTtyAsync(string id, ContainerResizeParameters parameters, CancellationToken cancellationToken = default)
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
        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/resize", queryParameters, cancellationToken);
    }

    public async Task<bool> StartContainerAsync(string id, ContainerStartParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        var queryParameters = parameters == null ? null : new QueryString<ContainerStartParameters>(parameters);
        bool? result = null;
        await _client.MakeRequestAsync(new[] { NoSuchContainerHandler, (statusCode, _) => result = statusCode != HttpStatusCode.NotModified }, HttpMethod.Post, $"containers/{id}/start", queryParameters, cancellationToken).ConfigureAwait(false);
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
        // since specified wait timespan can be greater than HttpClient's default, we set the
        // client timeout to infinite and provide a cancellation token.
        bool? result = null;
        await _client.MakeRequestAsync(new[] { NoSuchContainerHandler, (statusCode, _) => result = statusCode != HttpStatusCode.NotModified }, HttpMethod.Post, $"containers/{id}/stop", queryParameters, null, null, TimeSpan.FromMilliseconds(Timeout.Infinite), cancellationToken).ConfigureAwait(false);
        return result ?? throw new InvalidOperationException();
    }

    public Task RestartContainerAsync(string id, ContainerRestartParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }


        IQueryString queryParameters = new QueryString<ContainerRestartParameters>(parameters);
        // since specified wait timespan can be greater than HttpClient's default, we set the
        // client timeout to infinite and provide a cancellation token.
        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/restart", queryParameters, null, null, TimeSpan.FromMilliseconds(Timeout.Infinite), cancellationToken);
    }

    public Task KillContainerAsync(string id, ContainerKillParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString queryParameters = new QueryString<ContainerKillParameters>(parameters);
        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/kill", queryParameters, cancellationToken);
    }

    public Task RenameContainerAsync(string id, ContainerRenameParameters parameters, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        var queryParameters = new QueryString<ContainerRenameParameters>(parameters ?? throw new ArgumentNullException(nameof(parameters)));
        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/rename", queryParameters, cancellationToken);
    }

    public Task PauseContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/pause", cancellationToken);
    }

    public Task UnpauseContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/unpause", cancellationToken);
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

        var stream = await _client.MakeRequestForHijackedStreamAsync(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/attach", queryParameters, null, null, cancellationToken)
            .ConfigureAwait(false);

        return new MultiplexedStream(stream, !containerInspectResponse.Config.Tty);
    }

    public async Task<ContainerWaitResponse> WaitContainerAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<ContainerWaitResponse>(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/wait", null, null, null, Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
    }

    public Task RemoveContainerAsync(string id, ContainerRemoveParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString queryParameters = new QueryString<ContainerRemoveParameters>(parameters);
        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Delete, $"containers/{id}", queryParameters, cancellationToken);
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

        IQueryString queryParameters = new QueryString<ContainerPathStatParameters>(parameters);

        var response = await _client.MakeRequestForStreamedResponseAsync(new[] { NoSuchContainerHandler }, statOnly ? HttpMethod.Head : HttpMethod.Get, $"containers/{id}/archive", queryParameters, cancellationToken);

        var statHeader = response.Headers.GetValues("X-Docker-Container-Path-Stat").First();

        var bytes = Convert.FromBase64String(statHeader);

        var pathStat = DockerClient.JsonSerializer.Deserialize<ContainerPathStatResponse>(bytes);

        return new ContainerArchiveResponse
        {
            Stat = pathStat,
            Stream = statOnly ? null : response.Body
        };
    }

    public Task ExtractArchiveToContainerAsync(string id, ContainerPathStatParameters parameters, Stream stream, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString queryParameters = new QueryString<ContainerPathStatParameters>(parameters);

        var data = new BinaryRequestContent(stream, "application/x-tar");
        return _client.MakeRequestAsync(new[] { NoSuchContainerHandler }, HttpMethod.Put, $"containers/{id}/archive", queryParameters, data, cancellationToken);
    }

    public async Task<ContainersPruneResponse> PruneContainersAsync(ContainersPruneParameters parameters, CancellationToken cancellationToken)
    {
        var queryParameters = parameters == null ? null : new QueryString<ContainersPruneParameters>(parameters);
        return await _client.MakeRequestAsync<ContainersPruneResponse>(_client.NoErrorHandlers, HttpMethod.Post, "containers/prune", queryParameters, cancellationToken).ConfigureAwait(false);
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
        return await _client.MakeRequestAsync<ContainerUpdateResponse>(new[] { NoSuchContainerHandler }, HttpMethod.Post, $"containers/{id}/update", null, data, cancellationToken);
    }
}
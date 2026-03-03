namespace Docker.DotNet;

internal class VolumeOperations : IVolumeOperations
{
    private readonly DockerClient _client;

    internal VolumeOperations(DockerClient client)
    {
        _client = client;
    }

    public Task<VolumesListResponse> ListAsync(CancellationToken cancellationToken = default)
    {
        return ListAsync(null, cancellationToken);
    }

    public async Task<VolumesListResponse> ListAsync(VolumesListParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<VolumesListParameters>(parameters);
        return await _client.MakeRequestAsync<VolumesListResponse>(_client.NoErrorHandlers, HttpMethod.Get, "volumes", queryParameters, null, cancellationToken).ConfigureAwait(false);
    }

    public async Task<VolumeResponse> CreateAsync(VolumesCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var data = new JsonRequestContent<VolumesCreateParameters>(parameters, DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<VolumeResponse>(_client.NoErrorHandlers, HttpMethod.Post, "volumes/create", null, data, cancellationToken);
    }

    public async Task<VolumeResponse> InspectAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return await _client.MakeRequestAsync<VolumeResponse>(_client.NoErrorHandlers, HttpMethod.Get, $"volumes/{name}", cancellationToken).ConfigureAwait(false);
    }

    public Task RemoveAsync(string name, bool? force = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Delete, $"volumes/{name}", cancellationToken);
    }

    public async Task<VolumesPruneResponse> PruneAsync(VolumesPruneParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<VolumesPruneParameters>(parameters);
        return await _client.MakeRequestAsync<VolumesPruneResponse>(_client.NoErrorHandlers, HttpMethod.Post, "volumes/prune", queryParameters, cancellationToken);
    }
}
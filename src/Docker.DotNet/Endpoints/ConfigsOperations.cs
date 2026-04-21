namespace Docker.DotNet;

internal class ConfigOperations : IConfigOperations
{
    private readonly DockerClient _client;

    internal ConfigOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<IList<SwarmConfig>> ListConfigsAsync(CancellationToken cancellationToken = default)
    {
        return await _client.MakeRequestAsync<SwarmConfig[]>(_client.NoErrorHandlers, HttpMethod.Get, "configs", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<SwarmCreateConfigResponse> CreateConfigAsync(SwarmCreateConfigParameters body, CancellationToken cancellationToken = default)
    {
        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        var data = new JsonRequestContent<SwarmConfigSpec>(body.Config, DockerClient.JsonSerializer);

        return await _client.MakeRequestAsync<SwarmCreateConfigResponse>(_client.NoErrorHandlers, HttpMethod.Post, "configs/create", null, data, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<SwarmConfig> InspectConfigAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<SwarmConfig>(_client.NoErrorHandlers, HttpMethod.Get, $"configs/{id}", cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task RemoveConfigAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        await _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Delete, $"configs/{id}", cancellationToken)
            .ConfigureAwait(false);
    }
}
namespace Docker.DotNet;

internal class SecretsOperations : ISecretsOperations
{
    private readonly DockerClient _client;

    internal SecretsOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<IList<Secret>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _client.MakeRequestAsync<IList<Secret>>(_client.NoErrorHandlers, HttpMethod.Get, "secrets", cancellationToken).ConfigureAwait(false);
    }

    public async Task<SecretCreateResponse> CreateAsync(SwarmSecretSpec body, CancellationToken cancellationToken)
    {
        if (body == null)
        {
            throw new ArgumentNullException(nameof(body));
        }

        var data = new JsonRequestContent<SwarmSecretSpec>(body, DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<SecretCreateResponse>(_client.NoErrorHandlers, HttpMethod.Post, "secrets/create", null, data, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Secret> InspectAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<Secret>(_client.NoErrorHandlers, HttpMethod.Get, $"secrets/{id}", cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        await _client.MakeRequestAsync(_client.NoErrorHandlers, HttpMethod.Delete, $"secrets/{id}", cancellationToken).ConfigureAwait(false);
    }
}
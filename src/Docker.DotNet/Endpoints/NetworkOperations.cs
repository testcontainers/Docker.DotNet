namespace Docker.DotNet;

internal class NetworkOperations : INetworkOperations
{
    private static readonly ApiResponseErrorHandlingDelegate NoSuchNetworkHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.NotFound)
        {
            throw new DockerNetworkNotFoundException(statusCode, responseBody);
        }
    };

    private readonly DockerClient _client;

    internal NetworkOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<IList<NetworkResponse>> ListNetworksAsync(NetworksListParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<NetworksListParameters>(parameters);
        return await _client.MakeRequestAsync<NetworkResponse[]>(_client.NoErrorHandlers, HttpMethod.Get, "networks", queryParameters, cancellationToken).ConfigureAwait(false);
    }

    public async Task<NetworkResponse> InspectNetworkAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<NetworkResponse>([NoSuchNetworkHandler], HttpMethod.Get, $"networks/{id}", cancellationToken).ConfigureAwait(false);
    }

    public Task DeleteNetworkAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return _client.MakeRequestAsync([NoSuchNetworkHandler], HttpMethod.Delete, $"networks/{id}", cancellationToken);
    }

    public async Task<NetworksCreateResponse> CreateNetworkAsync(NetworksCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var data = new JsonRequestContent<NetworksCreateParameters>(parameters, DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<NetworksCreateResponse>(_client.NoErrorHandlers, HttpMethod.Post, "networks/create", null, data, cancellationToken).ConfigureAwait(false);
    }

    public Task ConnectNetworkAsync(string id, NetworkConnectParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (string.IsNullOrEmpty(parameters?.Container))
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var data = new JsonRequestContent<NetworkConnectParameters>(parameters, DockerClient.JsonSerializer);
        return _client.MakeRequestAsync([NoSuchNetworkHandler], HttpMethod.Post, $"networks/{id}/connect", null, data, cancellationToken);
    }

    public Task DisconnectNetworkAsync(string id, NetworkDisconnectParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (string.IsNullOrEmpty(parameters?.Container))
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var data = new JsonRequestContent<NetworkDisconnectParameters>(parameters, DockerClient.JsonSerializer);
        return _client.MakeRequestAsync([NoSuchNetworkHandler], HttpMethod.Post, $"networks/{id}/disconnect", null, data, cancellationToken);
    }

    public Task DeleteUnusedNetworksAsync(NetworksDeleteUnusedParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        return ((INetworkOperations)this).PruneNetworksAsync(parameters, cancellationToken);
    }

    public async Task<NetworksPruneResponse> PruneNetworksAsync(NetworksDeleteUnusedParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<NetworksDeleteUnusedParameters>(parameters);
        return await _client.MakeRequestAsync<NetworksPruneResponse>(_client.NoErrorHandlers, HttpMethod.Post, "networks/prune", queryParameters, cancellationToken);
    }
}
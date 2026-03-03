namespace Docker.DotNet;

internal class SwarmOperations : ISwarmOperations
{
    private static readonly ApiResponseErrorHandlingDelegate SwarmResponseHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.ServiceUnavailable)
        {
            // TODO: Make typed error.
            throw new Exception("Node is not part of a swarm.");
        }
    };

    private readonly DockerClient _client;

    internal SwarmOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<ServiceCreateResponse> CreateServiceAsync(ServiceCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        var data = new JsonRequestContent<ServiceSpec>(parameters.Service, DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<ServiceCreateResponse>(new[] { SwarmResponseHandler }, HttpMethod.Post, "services/create", null, data, RegistryAuthHeaders(parameters.RegistryAuth), cancellationToken).ConfigureAwait(false);
    }

    public async Task<SwarmUnlockResponse> GetSwarmUnlockKeyAsync(CancellationToken cancellationToken = default)
    {
        return await _client.MakeRequestAsync<SwarmUnlockResponse>(new[] { SwarmResponseHandler }, HttpMethod.Get, "swarm/unlockkey", cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> InitSwarmAsync(SwarmInitParameters parameters, CancellationToken cancellationToken = default)
    {
        var data = new JsonRequestContent<SwarmInitParameters>(parameters, DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<string>(
            new ApiResponseErrorHandlingDelegate[]
            {
                (statusCode, responseBody) =>
                {
                    if (statusCode == HttpStatusCode.NotAcceptable)
                    {
                        // TODO: Make typed error.
                        throw new Exception("Node is already part of a swarm.");
                    }
                }
            },
            HttpMethod.Post,
            "swarm/init",
            null,
            data,
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<SwarmService> InspectServiceAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

        return await _client.MakeRequestAsync<SwarmService>(new[] { SwarmResponseHandler }, HttpMethod.Get, $"services/{id}", cancellationToken).ConfigureAwait(false);
    }

    public async Task<SwarmInspectResponse> InspectSwarmAsync(CancellationToken cancellationToken = default)
    {
        return await _client.MakeRequestAsync<SwarmInspectResponse>(new[] { SwarmResponseHandler }, HttpMethod.Get, "swarm", cancellationToken).ConfigureAwait(false);
    }

    public async Task JoinSwarmAsync(SwarmJoinParameters parameters, CancellationToken cancellationToken = default)
    {
        var data = new JsonRequestContent<SwarmJoinParameters>(parameters, DockerClient.JsonSerializer);
        await _client.MakeRequestAsync(
            new ApiResponseErrorHandlingDelegate[]
            {
                (statusCode, responseBody) =>
                {
                    if (statusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        // TODO: Make typed error.
                        throw new Exception("Node is already part of a swarm.");
                    }
                }
            },
            HttpMethod.Post,
            "swarm/join",
            null,
            data,
            cancellationToken).ConfigureAwait(false);
    }

    public async Task LeaveSwarmAsync(SwarmLeaveParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<SwarmLeaveParameters>(parameters);
        await _client.MakeRequestAsync(
            new ApiResponseErrorHandlingDelegate[]
            {
                (statusCode, responseBody) =>
                {
                    if (statusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        // TODO: Make typed error.
                        throw new Exception("Node is not part of a swarm.");
                    }
                }
            },
            HttpMethod.Post,
            "swarm/leave",
            queryParameters,
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SwarmService>> ListServicesAsync(ServiceListParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<ServiceListParameters>(parameters);
        return await _client
            .MakeRequestAsync<SwarmService[]>(new[] { SwarmResponseHandler }, HttpMethod.Get, "services", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task RemoveServiceAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Delete, $"services/{id}", cancellationToken).ConfigureAwait(false);
    }

    public async Task UnlockSwarmAsync(SwarmUnlockParameters parameters, CancellationToken cancellationToken = default)
    {
        var body = new JsonRequestContent<SwarmUnlockParameters>(parameters, DockerClient.JsonSerializer);
        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Post, "swarm/unlock", null, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ServiceUpdateResponse> UpdateServiceAsync(string id, ServiceUpdateParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        var queryParameters = new QueryString<ServiceUpdateParameters>(parameters);
        var body = new JsonRequestContent<ServiceSpec>(parameters.Service, DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<ServiceUpdateResponse>(new[] { SwarmResponseHandler }, HttpMethod.Post, $"services/{id}/update", queryParameters, body, RegistryAuthHeaders(parameters.RegistryAuth), cancellationToken).ConfigureAwait(false);
    }

    public async Task<Stream> GetServiceLogsAsync(string id, ServiceLogsParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ServiceLogsParameters>(parameters);

        return await _client.MakeRequestForStreamAsync(new[] { SwarmResponseHandler }, HttpMethod.Get, $"services/{id}/logs", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<MultiplexedStream> GetServiceLogsAsync(string id, bool tty, ServiceLogsParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var queryParameters = new QueryString<ServiceLogsParameters>(parameters);

        var response = await _client.MakeRequestForStreamAsync(new[] { SwarmResponseHandler }, HttpMethod.Get, $"services/{id}/logs", queryParameters, cancellationToken).ConfigureAwait(false);

        return new MultiplexedStream(response, !tty);
    }

    public async Task UpdateSwarmAsync(SwarmUpdateParameters parameters, CancellationToken cancellationToken = default)
    {
        var queryParameters = new QueryString<SwarmUpdateParameters>(parameters);
        var body = new JsonRequestContent<Spec>(parameters.Spec, DockerClient.JsonSerializer);
        await _client.MakeRequestAsync(
            new ApiResponseErrorHandlingDelegate[]
            {
                (statusCode, responseBody) =>
                {
                    if (statusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        // TODO: Make typed error.
                        throw new Exception("Node is not part of a swarm.");
                    }
                }
            },
            HttpMethod.Post,
            "swarm/update",
            queryParameters,
            body,
            cancellationToken).ConfigureAwait(false);
    }

    private static Dictionary<string, string> RegistryAuthHeaders(AuthConfig? authConfig)
    {
        if (authConfig == null)
        {
            return new Dictionary<string, string>();
        }

        return new Dictionary<string, string>
        {
            {
                "X-Registry-Auth",
                Convert.ToBase64String(DockerClient.JsonSerializer.SerializeToUtf8Bytes(authConfig))
            }
        };
    }

    public async Task<IEnumerable<NodeListResponse>> ListNodesAsync(CancellationToken cancellationToken = default)
    {
        return await _client.MakeRequestAsync<NodeListResponse[]>(new[] { SwarmResponseHandler }, HttpMethod.Get, "nodes", cancellationToken).ConfigureAwait(false);
    }

    public async Task<NodeListResponse> InspectNodeAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        return await _client.MakeRequestAsync<NodeListResponse>(new[] { SwarmResponseHandler }, HttpMethod.Get, $"nodes/{id}", cancellationToken).ConfigureAwait(false);
    }

    public async Task RemoveNodeAsync(string id, bool? force = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        var parameters = new NodeRemoveParameters { Force = force };
        var queryParameters = new QueryString<NodeRemoveParameters>(parameters);
        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Delete, $"nodes/{id}", queryParameters, cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateNodeAsync(string id, ulong version, NodeUpdateParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        var queryParameters = new EnumerableQueryString("version", new[] { version.ToString() });
        var body = new JsonRequestContent<NodeUpdateParameters>(parameters, DockerClient.JsonSerializer);
        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Post, $"nodes/{id}/update", queryParameters, body, cancellationToken);
    }
}
namespace Docker.DotNet;

internal class SwarmOperations : ISwarmOperations
{
    internal static readonly ApiResponseErrorHandlingDelegate SwarmResponseHandler = (statusCode, responseBody) =>
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

    async Task<ServiceCreateResponse> ISwarmOperations.CreateServiceAsync(ServiceCreateParameters parameters, CancellationToken cancellationToken)
    {
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        var data = new JsonRequestContent<ServiceSpec>(parameters.Service ?? throw new ArgumentNullException(nameof(parameters.Service)), DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<ServiceCreateResponse>(new[] { SwarmResponseHandler }, HttpMethod.Post, "services/create", null, data, RegistryAuthHeaders(parameters.RegistryAuth), cancellationToken).ConfigureAwait(false);
    }

    async Task<SwarmUnlockResponse> ISwarmOperations.GetSwarmUnlockKeyAsync(CancellationToken cancellationToken)
    {
        return await _client.MakeRequestAsync<SwarmUnlockResponse>(new[] { SwarmResponseHandler }, HttpMethod.Get, "swarm/unlockkey", cancellationToken).ConfigureAwait(false);
    }

    async Task<string> ISwarmOperations.InitSwarmAsync(SwarmInitParameters parameters, CancellationToken cancellationToken)
    {
        var data = new JsonRequestContent<SwarmInitParameters>(parameters ?? throw new ArgumentNullException(nameof(parameters)), DockerClient.JsonSerializer);
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

    async Task<SwarmService> ISwarmOperations.InspectServiceAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

        return await _client.MakeRequestAsync<SwarmService>(new[] { SwarmResponseHandler }, HttpMethod.Get, $"services/{id}", cancellationToken).ConfigureAwait(false);
    }

    async Task<SwarmInspectResponse> ISwarmOperations.InspectSwarmAsync(CancellationToken cancellationToken)
    {
        return await _client.MakeRequestAsync<SwarmInspectResponse>(new[] { SwarmResponseHandler }, HttpMethod.Get, "swarm", cancellationToken).ConfigureAwait(false);
    }

    async Task ISwarmOperations.JoinSwarmAsync(SwarmJoinParameters parameters, CancellationToken cancellationToken)
    {
        var data = new JsonRequestContent<SwarmJoinParameters>(parameters ?? throw new ArgumentNullException(nameof(parameters)), DockerClient.JsonSerializer);
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

    async Task ISwarmOperations.LeaveSwarmAsync(SwarmLeaveParameters parameters, CancellationToken cancellationToken)
    {
        var query = parameters == null ? null : new QueryString<SwarmLeaveParameters>(parameters);
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
            query,
            cancellationToken).ConfigureAwait(false);
    }

    async Task<IEnumerable<SwarmService>> ISwarmOperations.ListServicesAsync(ServiceListParameters parameters, CancellationToken cancellationToken)
    {
        var queryParameters = parameters != null ? new QueryString<ServiceListParameters>(parameters) : null;
        return await _client
            .MakeRequestAsync<SwarmService[]>(new[] { SwarmResponseHandler }, HttpMethod.Get, "services", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    async Task ISwarmOperations.RemoveServiceAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));

        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Delete, $"services/{id}", cancellationToken).ConfigureAwait(false);
    }

    async Task ISwarmOperations.UnlockSwarmAsync(SwarmUnlockParameters parameters, CancellationToken cancellationToken)
    {
        var body = new JsonRequestContent<SwarmUnlockParameters>(parameters ?? throw new ArgumentNullException(nameof(parameters)), DockerClient.JsonSerializer);
        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Post, "swarm/unlock", null, body, cancellationToken).ConfigureAwait(false);
    }

    async Task<ServiceUpdateResponse> ISwarmOperations.UpdateServiceAsync(string id, ServiceUpdateParameters parameters, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        var query = new QueryString<ServiceUpdateParameters>(parameters);
        var body = new JsonRequestContent<ServiceSpec>(parameters.Service ?? throw new ArgumentNullException(nameof(parameters.Service)), DockerClient.JsonSerializer);
        return await _client.MakeRequestAsync<ServiceUpdateResponse>(new[] { SwarmResponseHandler }, HttpMethod.Post, $"services/{id}/update", query, body, RegistryAuthHeaders(parameters.RegistryAuth), cancellationToken).ConfigureAwait(false);
    }

    public Task<Stream> GetServiceLogsAsync(string id, ServiceLogsParameters parameters, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString queryParameters = new QueryString<ServiceLogsParameters>(parameters);
        return _client.MakeRequestForStreamAsync(new[] { SwarmResponseHandler }, HttpMethod.Get, $"services/{id}/logs", queryParameters, cancellationToken);
    }

    public async Task<MultiplexedStream> GetServiceLogsAsync(string id, bool tty, ServiceLogsParameters parameters, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        IQueryString queryParameters = new QueryString<ServiceLogsParameters>(parameters);

        Stream result = await _client.MakeRequestForStreamAsync(new[] { SwarmResponseHandler }, HttpMethod.Get, $"services/{id}/logs", queryParameters, cancellationToken).ConfigureAwait(false);

        return new MultiplexedStream(result, !tty);
    }

    async Task ISwarmOperations.UpdateSwarmAsync(SwarmUpdateParameters parameters, CancellationToken cancellationToken)
    {
        var query = new QueryString<SwarmUpdateParameters>(parameters ?? throw new ArgumentNullException(nameof(parameters)));
        var body = new JsonRequestContent<Spec>(parameters.Spec ?? throw new ArgumentNullException(nameof(parameters.Spec)), DockerClient.JsonSerializer);
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
            query,
            body,
            cancellationToken).ConfigureAwait(false);
    }

    private IDictionary<string, string> RegistryAuthHeaders(AuthConfig authConfig)
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

    async Task<IEnumerable<NodeListResponse>> ISwarmOperations.ListNodesAsync(CancellationToken cancellationToken)
    {
        return await _client.MakeRequestAsync<NodeListResponse[]>(new[] { SwarmResponseHandler }, HttpMethod.Get, "nodes", cancellationToken).ConfigureAwait(false);
    }

    async Task<NodeListResponse> ISwarmOperations.InspectNodeAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        return await _client.MakeRequestAsync<NodeListResponse>(new[] { SwarmResponseHandler }, HttpMethod.Get, $"nodes/{id}", cancellationToken).ConfigureAwait(false);
    }

    async Task ISwarmOperations.RemoveNodeAsync(string id, bool force, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        var parameters = new NodeRemoveParameters { Force = force };
        var query = new QueryString<NodeRemoveParameters>(parameters);
        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Delete, $"nodes/{id}", query, cancellationToken).ConfigureAwait(false);
    }

    async Task ISwarmOperations.UpdateNodeAsync(string id, ulong version, NodeUpdateParameters parameters, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
        var query = new EnumerableQueryString("version", new[] { version.ToString() });
        var body = new JsonRequestContent<NodeUpdateParameters>(parameters ?? throw new ArgumentNullException(nameof(parameters)), DockerClient.JsonSerializer);
        await _client.MakeRequestAsync(new[] { SwarmResponseHandler }, HttpMethod.Post, $"nodes/{id}/update", query, body, cancellationToken);
    }
}
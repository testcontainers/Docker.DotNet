namespace Docker.DotNet;

internal class ExecOperations : IExecOperations
{
    private static readonly ApiResponseErrorHandlingDelegate NoSuchContainerHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.NotFound)
        {
            throw new DockerContainerNotFoundException(statusCode, responseBody);
        }
    };

    private readonly DockerClient _client;

    internal ExecOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<ContainerExecInspectResponse> InspectContainerExecAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<ContainerExecInspectResponse>([NoSuchContainerHandler], HttpMethod.Get, $"exec/{id}/json", null, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ContainerExecCreateResponse> CreateContainerExecAsync(string id, ContainerExecCreateParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var data = new JsonRequestContent<ContainerExecCreateParameters>(parameters, DockerClient.JsonSerializer);

        return await _client.MakeRequestAsync<ContainerExecCreateResponse>([NoSuchContainerHandler], HttpMethod.Post, $"containers/{id}/exec", null, data, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<MultiplexedStream> StartContainerExecAsync(string id, ContainerExecStartParameters parameters, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        var data = new JsonRequestContent<ContainerExecStartParameters>(parameters, DockerClient.JsonSerializer);

        var stream = await _client.MakeRequestForHijackedStreamAsync([NoSuchContainerHandler], HttpMethod.Post, $"exec/{id}/start", null, data, null, cancellationToken)
            .ConfigureAwait(false);

        return new MultiplexedStream(stream, !parameters.TTY);
    }
}
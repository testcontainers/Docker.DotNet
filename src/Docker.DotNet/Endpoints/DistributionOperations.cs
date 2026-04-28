namespace Docker.DotNet;

internal class DistributionOperations : IDistributionOperations
{
    private static readonly ApiResponseErrorHandlingDelegate NoSuchImageHandler = (statusCode, responseBody) =>
    {
        if (statusCode == HttpStatusCode.NotFound)
        {
            throw new DockerImageNotFoundException(statusCode, responseBody);
        }
    };

    private readonly DockerClient _client;

    internal DistributionOperations(DockerClient client)
    {
        _client = client;
    }

    public async Task<DistributionInspectResponse> InspectAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        return await _client.MakeRequestAsync<DistributionInspectResponse>([NoSuchImageHandler], HttpMethod.Get, $"distribution/{name}/json", cancellationToken)
            .ConfigureAwait(false);
    }
}

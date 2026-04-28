namespace Docker.DotNet;

public interface IDistributionOperations
{
    /// <summary>
    /// Retrieves low-level information about an image from a registry.
    /// </summary>
    /// <param name="name">An image name or reference.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <remarks>
    /// The equivalent command in the Docker CLI is <c>docker manifest inspect</c>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerImageNotFoundException">No such image was found.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<DistributionInspectResponse> InspectAsync(string name, CancellationToken cancellationToken = default);
}

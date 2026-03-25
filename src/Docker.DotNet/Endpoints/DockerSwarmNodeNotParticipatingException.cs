namespace Docker.DotNet;

public class DockerSwarmNodeNotParticipatingException : DockerApiException
{
    public DockerSwarmNodeNotParticipatingException(HttpStatusCode statusCode, string? responseBody)
        : base(statusCode, responseBody)
    {
    }
}
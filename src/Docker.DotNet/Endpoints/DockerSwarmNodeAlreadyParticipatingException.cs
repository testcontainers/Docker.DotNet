namespace Docker.DotNet;

public class DockerSwarmNodeAlreadyParticipatingException : DockerApiException
{
    public DockerSwarmNodeAlreadyParticipatingException(HttpStatusCode statusCode, string? responseBody)
        : base(statusCode, responseBody)
    {
    }
}
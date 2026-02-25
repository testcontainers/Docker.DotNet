namespace Docker.DotNet;

public class DockerImageNotFoundException : DockerApiException
{
    public DockerImageNotFoundException(HttpStatusCode statusCode, string? responseBody)
        : base(statusCode, responseBody)
    {
    }
}
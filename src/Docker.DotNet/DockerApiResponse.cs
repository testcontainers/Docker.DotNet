namespace Docker.DotNet;

internal class DockerApiResponse
{
    public HttpStatusCode StatusCode { get; private set; }

    public string Body { get; private set; }

    public DockerApiResponse(HttpStatusCode statusCode, string body)
    {
        StatusCode = statusCode;
        Body = body;
    }
}
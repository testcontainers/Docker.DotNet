namespace Docker.DotNet;

public class DockerApiException : Exception
{
    public DockerApiException(HttpStatusCode statusCode, string? responseBody)
        : base($"Docker API responded with status code='{statusCode}', response='{responseBody}'.")
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    public HttpStatusCode StatusCode { get; }

    public string? ResponseBody { get; }
}
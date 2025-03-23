namespace Docker.DotNet;

internal sealed class DockerApiStreamedResponse
{
    public DockerApiStreamedResponse(HttpStatusCode statusCode, Stream body, HttpResponseHeaders headers)
    {
        StatusCode = statusCode;
        Body = body;
        Headers = headers;
    }

    public HttpStatusCode StatusCode { get; }

    public Stream Body { get; }

    public HttpResponseHeaders Headers { get; }
}
namespace Docker.DotNet;

public class AnonymousCredentials : Credentials
{
    public override void Dispose()
    {
    }

    public override bool IsTlsCredentials()
    {
        return false;
    }

    public override HttpMessageHandler GetHandler(HttpMessageHandler handler)
    {
        return handler;
    }
}
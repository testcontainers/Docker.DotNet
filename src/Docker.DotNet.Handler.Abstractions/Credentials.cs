namespace Docker.DotNet.Handler.Abstractions;

public abstract class Credentials : IDisposable
{
    public abstract void Dispose();

    public abstract bool IsTlsCredentials();

    public abstract HttpMessageHandler GetHandler(HttpMessageHandler handler);
}
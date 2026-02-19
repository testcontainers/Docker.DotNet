namespace Docker.DotNet.Handler.Abstractions;

[Obsolete("Use the IAuthProvider interface instead.")]
public abstract class Credentials : IDisposable
{
    public abstract void Dispose();

    public abstract bool IsTlsCredentials();

    public abstract HttpMessageHandler GetHandler(HttpMessageHandler handler);
}
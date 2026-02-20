namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// An authentication provider that does not use TLS and performs no authentication.
/// </summary>
public sealed class NoopAuthProvider : IAuthProvider
{
    private NoopAuthProvider()
    {
    }

    /// <summary>
    /// Gets the singleton instance.
    /// </summary>
    public static IAuthProvider Instance { get; }
        = new NoopAuthProvider();

    /// <inheritdoc />
    public bool TlsEnabled
        => false;

    /// <inheritdoc />
    public HttpMessageHandler ConfigureHandler(HttpMessageHandler handler)
        => handler;
}
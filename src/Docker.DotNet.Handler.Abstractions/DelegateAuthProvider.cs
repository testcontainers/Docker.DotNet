namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// An <see cref="IAuthProvider"/> that delegates to the configured <see cref="IDockerClientConfiguration"/> credentials.
/// </summary>
/// <param name="configuration">The client configuration.</param>
public sealed class DelegateAuthProvider(IDockerClientConfiguration configuration) : IAuthProvider
{
    /// <inheritdoc />
    public bool TlsEnabled
        => configuration.Credentials.IsTlsCredentials();

    /// <inheritdoc />
    public HttpMessageHandler ConfigureHandler(HttpMessageHandler handler)
        => configuration.Credentials.GetHandler(handler);
}
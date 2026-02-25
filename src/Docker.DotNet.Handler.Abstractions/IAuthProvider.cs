namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// Provides authentication and transport security configuration for a Docker client connection.
/// </summary>
public interface IAuthProvider
{
    /// <summary>
    /// Gets a value indicating whether TLS is enabled for the connection.
    /// </summary>
    bool TlsEnabled { get; }

    /// <summary>
    /// Configures and/or wraps the provided handler.
    /// </summary>
    /// <param name="handler">The base transport handler.</param>
    /// <returns>The configured handler.</returns>
    HttpMessageHandler ConfigureHandler(HttpMessageHandler handler);
}
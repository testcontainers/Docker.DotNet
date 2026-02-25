namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// Represents options used to configure a Docker client connection.
/// </summary>
public sealed record ClientOptions
{
    /// <summary>
    /// Gets the Docker Engine API version to request.
    /// </summary>
    public Version? ApiVersion { get; init; }

    /// <summary>
    /// Gets the endpoint URI to connect to.
    /// </summary>
    public Uri Endpoint { get; init; } = null!;

    /// <summary>
    /// Gets the authentication provider used to configure the HTTP handler.
    /// </summary>
    public IAuthProvider AuthProvider { get; init; } = NoopAuthProvider.Instance;

    /// <summary>
    /// Gets default HTTP headers applied to each request.
    /// </summary>
    public IReadOnlyDictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets the maximum time to wait for an HTTP request to complete.
    /// </summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(100);
}
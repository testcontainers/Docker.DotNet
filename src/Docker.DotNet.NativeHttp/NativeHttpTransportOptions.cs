namespace Docker.DotNet.NativeHttp;

/// <summary>
/// Represents transport options for the native HTTP transport.
/// </summary>
public sealed record NativeHttpTransportOptions
{
#if NET6_0_OR_GREATER
    /// <summary>
    /// Gets a callback that configures the created <see cref="SocketsHttpHandler"/> instance.
    /// </summary>
    public Action<SocketsHttpHandler> ConfigureHandler { get; init; } = _ => { };
#else
    /// <summary>
    /// Gets a callback that configures the created <see cref="HttpClientHandler"/> instance.
    /// </summary>
    public Action<HttpClientHandler> ConfigureHandler { get; init; } = _ => { };
#endif
}
namespace Docker.DotNet.LegacyHttp;

/// <summary>
/// Represents transport options for the legacy HTTP transport.
/// </summary>
public sealed record LegacyHttpTransportOptions
{
    /// <summary>
    /// Gets a callback that configures the created <see cref="ManagedHandler"/> instance.
    /// </summary>
    public Action<ManagedHandler> ConfigureHandler { get; init; } = _ => { };
}
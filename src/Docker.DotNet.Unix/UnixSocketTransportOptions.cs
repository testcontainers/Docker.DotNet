namespace Docker.DotNet.Unix;

/// <summary>
/// Represents transport options for the Unix domain socket transport.
/// </summary>
public sealed record UnixSocketTransportOptions
{
    /// <summary>
    /// Gets a callback that configures the created <see cref="ManagedHandler"/> instance.
    /// </summary>
    public Action<ManagedHandler> ConfigureHandler { get; init; } = _ => { };
}
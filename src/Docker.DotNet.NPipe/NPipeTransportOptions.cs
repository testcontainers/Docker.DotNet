namespace Docker.DotNet.NPipe;

/// <summary>
/// Represents transport options for the Windows named pipe transport.
/// </summary>
public sealed record NPipeTransportOptions
{
    /// <summary>
    /// Gets the maximum time to wait for the named pipe connection to be established.
    /// </summary>
    public TimeSpan ConnectTimeout { get; init; } = TimeSpan.FromSeconds(10);
}
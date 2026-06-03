namespace Docker.DotNet;

/// <summary>
/// Represents Docker CLI settings.
/// </summary>
public interface IDockerCliSettings
{
    /// <summary>
    /// Gets the Docker configuration directory.
    /// </summary>
    string? DockerConfig { get; }

    /// <summary>
    /// Gets the Docker host.
    /// </summary>
    string? DockerHost { get; }

    /// <summary>
    /// Gets a value indicating whether Docker TLS verify is enabled.
    /// </summary>
    string? DockerTlsVerify { get; }

    /// <summary>
    /// Gets the Docker context.
    /// </summary>
    string? DockerContext { get; }
}
namespace Docker.DotNet;

/// <summary>
/// Represents Docker CLI settings resolved from environment variables.
/// </summary>
internal sealed class EnvironmentDockerCliSettings : IDockerCliSettings
{
    static EnvironmentDockerCliSettings()
    {
    }

    private EnvironmentDockerCliSettings()
    {
    }

    /// <summary>
    /// Gets the <see cref="EnvironmentDockerCliSettings"/> instance.
    /// </summary>
    public static EnvironmentDockerCliSettings Instance { get; } = new();

    /// <inheritdoc />
    public string? DockerConfig
        => Environment.GetEnvironmentVariable("DOCKER_CONFIG");

    /// <inheritdoc />
    public string? DockerHost
        => Environment.GetEnvironmentVariable("DOCKER_HOST");

    /// <inheritdoc />
    public string? DockerTlsVerify
        => Environment.GetEnvironmentVariable("DOCKER_TLS_VERIFY");

    /// <inheritdoc />
    public string? DockerContext
        => Environment.GetEnvironmentVariable("DOCKER_CONTEXT");
}
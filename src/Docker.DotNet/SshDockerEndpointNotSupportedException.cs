namespace Docker.DotNet;

/// <summary>
/// Represents an exception that is thrown when a Docker endpoint uses the
/// SSH transport, which is not supported.
/// </summary>
public sealed class SshDockerEndpointNotSupportedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SshDockerEndpointNotSupportedException" /> class.
    /// </summary>
    public SshDockerEndpointNotSupportedException()
        : base("Docker endpoints using SSH are not supported. Use TCP, HTTPS, Unix socket, or named pipe endpoints, or connect through an SSH tunnel.")
    {
    }
}
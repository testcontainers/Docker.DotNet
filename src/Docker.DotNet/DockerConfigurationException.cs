namespace Docker.DotNet;

/// <summary>
/// Represents an exception that is thrown when the Docker configuration file
/// cannot be read successfully.
/// </summary>
public sealed class DockerConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DockerConfigurationException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DockerConfigurationException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerConfigurationException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public DockerConfigurationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
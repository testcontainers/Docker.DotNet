namespace Docker.DotNet.Handler.Abstractions;

[Obsolete("Use the DockerClientBuilder class instead: https://github.com/testcontainers/Docker.DotNet/blob/main/README.md#usage.")]
public interface IDockerClientConfiguration
{
    /// <summary>
    /// Gets the collection of default HTTP request headers.
    /// </summary>
    public IReadOnlyDictionary<string, string> DefaultHttpRequestHeaders { get; }

    public Uri EndpointBaseUri { get; }

    public Credentials Credentials { get; }

    public TimeSpan DefaultTimeout { get; }

    public TimeSpan NamedPipeConnectTimeout { get; }
}
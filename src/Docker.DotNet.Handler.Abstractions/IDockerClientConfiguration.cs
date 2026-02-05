namespace Docker.DotNet.Handler.Abstractions;

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

    /// <summary>
    /// Gets the socket connection configuration applied to Unix domain socket and TCP connections.
    /// </summary>
    public SocketConnectionConfiguration SocketConfiguration { get; }

    /// <summary>
    /// Gets an optional callback that is invoked with the <see cref="HttpMessageHandler"/> after
    /// it is created by the handler factory. Use this to apply custom configurations to the handler.
    /// </summary>
    public Action<HttpMessageHandler> ConfigureHandler { get; }
}

namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// Tracks the socket of the connection that carries a hijacked stream.
/// </summary>
/// <remarks>
/// The client begins a scope before it sends a request whose connection it
/// hijacks. A transport that owns its sockets tracks the socket the request is
/// written to, so it can close only the write side of the hijacked stream.
/// </remarks>
internal sealed class HijackScope : IDisposable
{
    private static readonly AsyncLocal<HijackScope?> CurrentScope = new();

    private HijackScope()
    {
    }

    /// <summary>
    /// Gets the scope of the current asynchronous control flow, or
    /// <see langword="null"/> if none was begun.
    /// </summary>
    public static HijackScope? Current
        => CurrentScope.Value;

    /// <summary>
    /// Gets the socket the request was last written to, or
    /// <see langword="null"/> if none was tracked.
    /// </summary>
    public Socket? Socket { get; private set; }

    /// <summary>
    /// Begins a scope for the current asynchronous control flow.
    /// </summary>
    /// <returns>The scope, which ends when it is disposed.</returns>
    public static HijackScope Begin()
    {
        var scope = new HijackScope();
        CurrentScope.Value = scope;
        return scope;
    }

    /// <summary>
    /// Tracks the socket the request is written to.
    /// </summary>
    /// <param name="socket">The socket.</param>
    /// <remarks>
    /// The last tracked socket wins, because a request that fails
    /// on a pooled connection is retried on a new one.
    /// </remarks>
    public void TrackSocket(Socket socket)
    {
        Socket = socket;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        CurrentScope.Value = null;
    }
}
namespace Docker.DotNet.Handler.Abstractions;

/// <summary>
/// Represents a transport-specific HTTP handler and the effective endpoint used for requests.
/// </summary>
/// <param name="Handler">The configured HTTP message handler.</param>
/// <param name="EffectiveEndpoint">The normalized endpoint URI used for requests.</param>
public sealed record ResolvedTransport(HttpMessageHandler Handler, Uri EffectiveEndpoint);
namespace Docker.DotNet;

using System.Security.Cryptography;

/// <summary>
/// Resolves the Docker daemon endpoint the same way the <c>docker</c> CLI does:
/// environment variables first, then the active Docker context, then the
/// platform default socket. Works on Linux, macOS, and Windows.
/// </summary>
public static class DockerContextResolver
{
    private const string DefaultContextName = "default";

    private const string DockerEndpointKey = "docker";

    /// <summary>
    /// Resolves the endpoint URI to use for the Docker daemon, following the
    /// docker CLI lookup order:
    /// <list type="number">
    ///   <item><description><c>DOCKER_HOST</c> (with <c>DOCKER_TLS_VERIFY</c> upgrading <c>tcp://</c> to <c>https://</c>).</description></item>
    ///   <item><description><c>DOCKER_CONTEXT</c>, or <c>currentContext</c> from <c>~/.docker/config.json</c>.</description></item>
    ///   <item><description>The platform default socket (<c>unix:/var/run/docker.sock</c> on Linux/macOS, <c>npipe://./pipe/docker_engine</c> on Windows).</description></item>
    /// </list>
    /// <c>DOCKER_CONFIG</c> is honored to override the configuration directory.
    /// </summary>
    /// <returns>The resolved endpoint URI.</returns>
    public static Uri Resolve()
    {
        var host = Environment.GetEnvironmentVariable("DOCKER_HOST");
        if (!string.IsNullOrEmpty(host))
        {
            return BuildHostUri(host, IsTlsVerifyEnabled());
        }

        var contextName = Environment.GetEnvironmentVariable("DOCKER_CONTEXT");
        if (string.IsNullOrEmpty(contextName))
        {
            contextName = TryReadCurrentContext();
        }

        if (!string.IsNullOrEmpty(contextName) && !string.Equals(contextName, DefaultContextName, StringComparison.Ordinal))
        {
            var contextHost = TryReadContextHost(contextName!);
            if (!string.IsNullOrEmpty(contextHost))
            {
                return BuildHostUri(contextHost!, IsTlsVerifyEnabled());
            }
        }

        return GetPlatformDefault();
    }

    /// <summary>
    /// Resolves the endpoint URI for the named Docker context, reading it from
    /// <c>~/.docker/contexts/meta/&lt;sha256(name)&gt;/meta.json</c>.
    /// </summary>
    /// <param name="contextName">The context name (e.g. <c>desktop-linux</c>).</param>
    /// <returns>The endpoint URI declared by the context.</returns>
    public static Uri ResolveForContext(string contextName)
    {
        if (string.IsNullOrEmpty(contextName))
        {
            throw new ArgumentException("Context name must be provided", nameof(contextName));
        }

        if (string.Equals(contextName, DefaultContextName, StringComparison.Ordinal))
        {
            return GetPlatformDefault();
        }

        var host = TryReadContextHost(contextName);
        if (string.IsNullOrEmpty(host))
        {
            throw new InvalidOperationException($"Docker context '{contextName}' was not found under '{GetContextsMetaDirectory()}'.");
        }

        return BuildHostUri(host!, tlsVerify: false);
    }

    private static bool IsTlsVerifyEnabled()
    {
        var value = Environment.GetEnvironmentVariable("DOCKER_TLS_VERIFY");
        return !string.IsNullOrEmpty(value);
    }

    private static Uri BuildHostUri(string host, bool tlsVerify)
    {
        if (tlsVerify && host.StartsWith("tcp://", StringComparison.OrdinalIgnoreCase))
        {
            host = "https://" + host.Substring("tcp://".Length);
        }

        return new Uri(host);
    }

    private static string? TryReadCurrentContext()
    {
        var configPath = Path.Combine(GetDockerConfigDirectory(), "config.json");
        if (!File.Exists(configPath))
        {
            return null;
        }

        try
        {
            using var stream = File.OpenRead(configPath);
            using var document = JsonDocument.Parse(stream);
            if (document.RootElement.TryGetProperty("currentContext", out var element) && element.ValueKind == JsonValueKind.String)
            {
                return element.GetString();
            }
        }
        catch (JsonException)
        {
        }
        catch (IOException)
        {
        }

        return null;
    }

    private static string? TryReadContextHost(string contextName)
    {
        var metaPath = Path.Combine(GetContextsMetaDirectory(), Sha256Hex(contextName), "meta.json");
        if (!File.Exists(metaPath))
        {
            return null;
        }

        try
        {
            using var stream = File.OpenRead(metaPath);
            using var document = JsonDocument.Parse(stream);
            if (!document.RootElement.TryGetProperty("Endpoints", out var endpoints) || endpoints.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            if (!endpoints.TryGetProperty(DockerEndpointKey, out var dockerEndpoint) || dockerEndpoint.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            if (dockerEndpoint.TryGetProperty("Host", out var hostElement) && hostElement.ValueKind == JsonValueKind.String)
            {
                return hostElement.GetString();
            }
        }
        catch (JsonException)
        {
        }
        catch (IOException)
        {
        }

        return null;
    }

    private static string GetDockerConfigDirectory()
    {
        var dockerConfig = Environment.GetEnvironmentVariable("DOCKER_CONFIG");
        if (!string.IsNullOrEmpty(dockerConfig))
        {
            return dockerConfig!;
        }

        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".docker");
    }

    private static string GetContextsMetaDirectory()
    {
        return Path.Combine(GetDockerConfigDirectory(), "contexts", "meta");
    }

    private static string Sha256Hex(string value)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
        var builder = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }

    private static Uri GetPlatformDefault()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new Uri("npipe://./pipe/docker_engine")
            : new Uri("unix:/var/run/docker.sock");
    }
}

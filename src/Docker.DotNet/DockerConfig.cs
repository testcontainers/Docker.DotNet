namespace Docker.DotNet;

/// <summary>
/// Represents a Docker config.
/// </summary>
public sealed class DockerConfig
{
    private const string DefaultDockerContext = "default";

    private static readonly string UserProfileDockerConfigDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".docker");

    private static readonly Uri WindowsDockerEngine = new("npipe://./pipe/docker_engine");

    private static readonly Uri UnixDockerEngine = new("unix:///var/run/docker.sock");

    private readonly object _contextEndpointsSync = new();

    private readonly Dictionary<string, Uri> _contextEndpoints = new(StringComparer.Ordinal);

    private readonly IDockerCliSettings _settings;

    private readonly string _dockerConfigDirectoryPath;

    private readonly string _dockerConfigFilePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerConfig" /> class.
    /// </summary>
    public DockerConfig()
        : this(EnvironmentDockerCliSettings.Instance)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DockerConfig" /> class.
    /// </summary>
    /// <param name="settings">The Docker CLI settings.</param>
    public DockerConfig(IDockerCliSettings settings)
    {
        _settings = settings;
        _dockerConfigDirectoryPath = GetDockerConfig();
        _dockerConfigFilePath = Path.Combine(_dockerConfigDirectoryPath, "config.json");
    }

    /// <summary>
    /// Gets the <see cref="DockerConfig" /> instance.
    /// </summary>
    public static DockerConfig Instance { get; } = new();

    /// <inheritdoc cref="FileSystemInfo.Exists" />
    public bool Exists => File.Exists(_dockerConfigFilePath);

    /// <inheritdoc cref="FileSystemInfo.FullName" />
    public string FullName => _dockerConfigFilePath;

    /// <summary>
    /// Parses the Docker config file.
    /// </summary>
    /// <returns>A <see cref="JsonDocument" /> representing the Docker config.</returns>
    public JsonDocument Parse()
    {
        using var dockerConfigFileStream = File.OpenRead(_dockerConfigFilePath);
        return JsonDocument.Parse(dockerConfigFileStream);
    }

    /// <summary>
    /// Gets the current Docker endpoint.
    /// </summary>
    /// <remarks>
    /// See the Docker CLI implementation <a href="https://github.com/docker/cli/blob/v25.0.0/cli/command/cli.go#L364-L390">comments</a>.
    /// Executes a command equivalent to <c>docker context inspect --format {{.Endpoints.docker.Host}}</c>.
    /// </remarks>
    /// <returns>A <see cref="Uri" /> representing the current Docker endpoint.</returns>
    public Uri GetCurrentEndpoint()
    {
        var dockerHost = GetDockerHost();
        if (dockerHost is not null)
        {
            return dockerHost;
        }

        var dockerContext = GetCurrentContext();
        if (dockerContext is null)
        {
            return GetPlatformDefaultEndpoint();
        }

        if (dockerContext.Length == 0 || DefaultDockerContext.Equals(dockerContext))
        {
            return GetPlatformDefaultEndpoint();
        }

        return GetEndpoint(dockerContext);
    }

    /// <summary>
    /// Gets the Docker endpoint declared by the named Docker context.
    /// </summary>
    /// <param name="contextName">The Docker context name.</param>
    /// <returns>A <see cref="Uri" /> representing the Docker endpoint for the context.</returns>
    public Uri GetEndpoint(string contextName)
    {
        if (string.IsNullOrWhiteSpace(contextName))
        {
            throw new ArgumentException("The Docker context name cannot be null or empty.", nameof(contextName));
        }

        if (DefaultDockerContext.Equals(contextName))
        {
            return GetPlatformDefaultEndpoint();
        }

        lock (_contextEndpointsSync)
        {
            if (_contextEndpoints.TryGetValue(contextName, out var cachedEndpoint))
            {
                return cachedEndpoint;
            }
        }

        var endpoint = LoadEndpoint(contextName);

        lock (_contextEndpointsSync)
        {
            _contextEndpoints[contextName] = endpoint;
        }

        return endpoint;
    }

    private Uri LoadEndpoint(string contextName)
    {
        var metaFilePath = GetContextMetaFilePath(contextName);

        try
        {
            using var metaFileStream = File.OpenRead(metaFilePath);
            var meta = System.Text.Json.JsonSerializer.Deserialize(metaFileStream, SourceGenerationContext.Default.DockerContextMeta);
            var host = meta?.Endpoints?.Docker?.Host;

            if (host is null || host.Length == 0)
            {
                throw new DockerConfigurationException($"The Docker host is null or empty in '{metaFilePath}' (JSONPath: Endpoints.docker.Host).");
            }

            return new Uri(host.Replace("npipe:////./", "npipe://./"));
        }
        catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
        {
            throw new DockerConfigurationException($"The Docker context '{contextName}' does not exist.", e);
        }
        catch (Exception e) when (e is not DockerConfigurationException)
        {
            throw new DockerConfigurationException($"The Docker context '{contextName}' failed to load from '{metaFilePath}'.", e);
        }
    }

    private string GetContextMetaFilePath(string contextName)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contextName));

#if NET10_0_OR_GREATER
        var dockerContextHash = Convert.ToHexStringLower(hashBytes);
#else
        var dockerContextHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();
#endif

        return Path.Combine(_dockerConfigDirectoryPath, "contexts", "meta", dockerContextHash, "meta.json");
    }

    private string? GetCurrentContext()
    {
        var dockerContext = GetDockerContext();
        if (!string.IsNullOrEmpty(dockerContext))
        {
            return dockerContext;
        }

        if (!Exists)
        {
            return null;
        }

        using var dockerConfigJsonDocument = Parse();

        if (dockerConfigJsonDocument.RootElement.TryGetProperty("currentContext", out var currentContext) && currentContext.ValueKind == JsonValueKind.String)
        {
            return currentContext.GetString();
        }
        else
        {
            return null;
        }
    }

    private string GetDockerConfig()
    {
        var dockerConfig = _settings.DockerConfig;
        if (dockerConfig is null || dockerConfig.Length == 0)
        {
            return UserProfileDockerConfigDirectoryPath;
        }

        return dockerConfig;
    }

    private Uri? GetDockerHost()
    {
        if (string.IsNullOrEmpty(_settings.DockerHost))
        {
            return null;
        }

        if (!Uri.TryCreate(_settings.DockerHost, UriKind.Absolute, out var dockerHost))
        {
            throw new DockerConfigurationException($"The Docker host '{_settings.DockerHost}' is invalid.");
        }

        return dockerHost;
    }

    private string? GetDockerContext()
    {
        var dockerContext = _settings.DockerContext;
        if (dockerContext is null || dockerContext.Length == 0)
        {
            return null;
        }

        return dockerContext;
    }

    private static Uri GetPlatformDefaultEndpoint()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? WindowsDockerEngine : UnixDockerEngine;
    }

    internal sealed class DockerContextMeta
    {
        [JsonConstructor]
        public DockerContextMeta(DockerContextMetaEndpoints endpoints)
        {
            Endpoints = endpoints;
        }

        [JsonPropertyName("Endpoints")]
        public DockerContextMetaEndpoints? Endpoints { get; }
    }

    internal sealed class DockerContextMetaEndpoints
    {
        [JsonConstructor]
        public DockerContextMetaEndpoints(DockerContextMetaEndpointsDocker docker)
        {
            Docker = docker;
        }

        [JsonPropertyName("docker")]
        public DockerContextMetaEndpointsDocker? Docker { get; }
    }

    internal sealed class DockerContextMetaEndpointsDocker
    {
        [JsonConstructor]
        public DockerContextMetaEndpointsDocker(string host)
        {
            Host = host;
        }

        [JsonPropertyName("Host")]
        public string Host { get; }
    }
}
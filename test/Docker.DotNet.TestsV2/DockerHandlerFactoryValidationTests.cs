namespace Docker.DotNet.TestsV2;

public sealed class DockerHandlerFactoryValidationTests
{
    public static TheoryData<Func<ResolvedClientOptions, ResolvedTransport>, Uri, string, string>
        InvalidSchemeCases =>
        new()
        {
            {
                options => LegacyHttp.DockerHandlerFactory.Instance.CreateHandler(options, NullLogger.Instance),
                new Uri("npipe://./pipe/docker_engine"),
                "LegacyHttpTransportOptions",
                "tcp', 'http', or 'https"
            },
            {
                options => NativeHttp.DockerHandlerFactory.Instance.CreateHandler(options, NullLogger.Instance),
                new Uri("npipe://./pipe/docker_engine"),
                "NativeHttpTransportOptions",
                "tcp', 'http', 'https', or 'unix"
            },
            {
                options => NPipe.DockerHandlerFactory.Instance.CreateHandler(options, NullLogger.Instance),
                new Uri("http://localhost:2375"),
                "NPipeTransportOptions",
                "scheme 'npipe'"
            },
            {
                options => Unix.DockerHandlerFactory.Instance.CreateHandler(options, NullLogger.Instance),
                new Uri("http://localhost:2375"),
                "UnixSocketTransportOptions",
                "scheme 'unix'"
            },
        };

    [Theory]
    [MemberData(nameof(InvalidSchemeCases))]
    public void CreateHandler_ThrowsInvalidOperationException_WhenSchemeDoesNotMatchTransport(
        Func<ResolvedClientOptions, ResolvedTransport> createHandler,
        Uri endpoint,
        string transportOptionsName,
        string expectedSchemesFragment)
    {
        var clientOptions = new ResolvedClientOptions { Endpoint = endpoint };

        var exception = Assert.Throws<InvalidOperationException>(() => createHandler(clientOptions));

        Assert.Contains(transportOptionsName, exception.Message, StringComparison.Ordinal);
        Assert.Contains(expectedSchemesFragment, exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    public static TheoryData<string> NamedPipeEndpoints =>
        new()
        {
            // The two-slash form System.Uri parses with a pipe server host.
            "npipe://./pipe/docker_engine",
            // The four-slash form Docker documents for DOCKER_HOST (equivalent to \\.\pipe\docker_engine),
            // which System.Uri parses with an empty host: https://github.com/testcontainers/Docker.DotNet/issues/135.
            "npipe:////./pipe/docker_engine",
            "npipe:////localhost/pipe/docker_engine",
            "npipe:////docker-host/pipe/docker_engine",
        };

    [Theory]
    [MemberData(nameof(NamedPipeEndpoints))]
    public void CreateHandler_AcceptsDockerDocumentedNamedPipeEndpoints(string endpoint)
    {
        var clientOptions = new ResolvedClientOptions { Endpoint = new Uri(endpoint) };

        var transport = NPipe.DockerHandlerFactory.Instance.CreateHandler(clientOptions, NullLogger.Instance);

        Assert.Equal(new Uri("http://docker_engine"), transport.EffectiveEndpoint);
    }

    public static TheoryData<string> MalformedNamedPipeEndpoints =>
        new()
        {
            "npipe:////./pipe/",
            "npipe:////./docker_engine",
            "npipe:////pipe/docker_engine",
            "npipe:////./pipe/engine/docker_engine",
        };

    [Theory]
    [MemberData(nameof(MalformedNamedPipeEndpoints))]
    public void CreateHandler_ThrowsInvalidOperationException_WhenNamedPipeEndpointIsMalformed(string endpoint)
    {
        var clientOptions = new ResolvedClientOptions { Endpoint = new Uri(endpoint) };

        var exception = Assert.Throws<InvalidOperationException>(() => NPipe.DockerHandlerFactory.Instance.CreateHandler(clientOptions, NullLogger.Instance));

        Assert.Equal("The endpoint is not a npipe URI.", exception.Message);
    }

    public static TheoryData<string, string, string> NamedPipeEndpointComponents =>
        new()
        {
            // npipe://SERVER/pipe/NAME, with the pipe server in the URI host.
            { "npipe://./pipe/docker_engine", ".", "docker_engine" },
            { "npipe://localhost/pipe/docker_engine", ".", "docker_engine" },
            { "npipe://docker-host/pipe/docker_engine", "docker-host", "docker_engine" },
            // npipe:////SERVER/pipe/NAME, the form Docker documents for DOCKER_HOST.
            { "npipe:////./pipe/docker_engine", ".", "docker_engine" },
            { "npipe:////localhost/pipe/docker_engine", ".", "docker_engine" },
            { "npipe:////docker-host/pipe/docker_engine", "docker-host", "docker_engine" },
            { "npipe:////./pipe/dockerDesktopLinuxEngine", ".", "dockerDesktopLinuxEngine" },
            { "NPIPE:////./PIPE/docker_engine", ".", "docker_engine" },
        };

    [Theory]
    [MemberData(nameof(NamedPipeEndpointComponents))]
    public void TryParsePipeEndpoint_ReadsServerNameAndPipeName(string endpoint, string expectedServerName, string expectedPipeName)
    {
        Assert.True(NPipe.DockerHandlerFactory.TryParsePipeEndpoint(new Uri(endpoint), out var serverName, out var pipeName));

        Assert.Equal(expectedServerName, serverName);
        Assert.Equal(expectedPipeName, pipeName);
    }
}
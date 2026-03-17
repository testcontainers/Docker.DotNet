namespace Docker.DotNet.TestsV2;

public sealed class DockerClientBuilderTests
{
    [Fact]
    public void Constructor_SetsPlatformDefaultEndpoint()
    {
        var builder = new TestDockerClientBuilder();

        var expected = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new Uri("npipe://./pipe/docker_engine")
            : new Uri("unix:/var/run/docker.sock");

        Assert.Equal(expected, builder.ClientOptions.Endpoint);
    }

    [Fact]
    public void WithApiVersion_UpdatesClientOptions()
    {
        var version = new Version(1, 52);
        var builder = new TestDockerClientBuilder();

        var sameInstance = builder.WithApiVersion(version);

        Assert.Same(builder, sameInstance);
        Assert.Equal(version, builder.ClientOptions.ApiVersion);
    }

    [Fact]
    public void WithEndpoint_UpdatesClientOptions()
    {
        var endpoint = new Uri("http://localhost:2375");
        var builder = new TestDockerClientBuilder();

        var sameInstance = builder.WithEndpoint(endpoint);

        Assert.Same(builder, sameInstance);
        Assert.Equal(endpoint, builder.ClientOptions.Endpoint);
    }

    [Fact]
    public void WithAuthProvider_UpdatesClientOptions()
    {
        var authProvider = new PassThroughAuthProvider(true);
        var builder = new TestDockerClientBuilder();

        var sameInstance = builder.WithAuthProvider(authProvider);

        Assert.Same(builder, sameInstance);
        Assert.Same(authProvider, builder.ClientOptions.AuthProvider);
    }

    [Fact]
    public void WithHeader_AddsAndReplacesHeader()
    {
        var builder = new TestDockerClientBuilder();

        _ = builder.WithHeader("x-one", "1");
        _ = builder.WithHeader("x-two", "2");
        _ = builder.WithHeader("x-one", "1b");

        Assert.Equal("1b", builder.ClientOptions.Headers["x-one"]);
        Assert.Equal("2", builder.ClientOptions.Headers["x-two"]);
        Assert.Equal(2, builder.ClientOptions.Headers.Count);
    }

    [Fact]
    public void WithHeaders_MergesAndReplacesHeaders()
    {
        var headers = new Dictionary<string, string>();
        headers["x-one"] = "1b";
        headers["x-two"] = "2";

        var builder = new TestDockerClientBuilder();

        _ = builder.WithHeader("x-one", "1");
        _ = builder.WithHeaders(headers);

        Assert.Equal("1b", builder.ClientOptions.Headers["x-one"]);
        Assert.Equal("2", builder.ClientOptions.Headers["x-two"]);
        Assert.Equal(2, builder.ClientOptions.Headers.Count);
    }

    [Fact]
    public void WithTimeout_UpdatesClientOptions()
    {
        var timeout = TimeSpan.FromSeconds(1);
        var builder = new TestDockerClientBuilder();

        var sameInstance = builder.WithTimeout(timeout);

        Assert.Same(builder, sameInstance);
        Assert.Equal(timeout, builder.ClientOptions.Timeout);
    }

    [Fact]
    public void WithLogger_UpdatesLogger()
    {
        var logger = NullLogger.Instance;
        var builder = new TestDockerClientBuilder();

        var sameInstance = builder.WithLogger(logger);

        Assert.Same(builder, sameInstance);
        Assert.Same(logger, builder.Logger);
    }

    [Fact]
    public void Build_WithUnsupportedScheme_ThrowsNotSupportedException()
    {
        var builder = new DockerClientBuilder()
            .WithEndpoint(new Uri("ssh://docker-host"));

        var exception = Assert.Throws<NotSupportedException>(() => builder.Build());

        Assert.Contains("ssh", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("npipe://./pipe/docker_engine", typeof(NPipe.DockerHandlerFactory))]
    [InlineData("unix:/var/run/docker.sock", typeof(Unix.DockerHandlerFactory))]
    public void ResolveTransportFactory_UsesExpectedFactory_WhenSocketSchemeIsProvided(string endpoint, Type expectedFactoryType)
    {
        var builder = new TestDockerClientBuilder();

        var actualFactory = builder.ResolveTransportFactory(new Uri(endpoint));

        Assert.IsType(expectedFactoryType, actualFactory);
    }

    [Fact]
    public void ResolveTransportFactory_UsesExpectedFactory_WhenHttpSchemeIsProvided()
    {
        var builder = new TestDockerClientBuilder();

        var actualFactory = builder.ResolveTransportFactory(new Uri("http://localhost:2375"));

        var expectedFactoryType = Environment.GetEnvironmentVariable("DOCKER_DOTNET_NATIVE_HTTP_ENABLED") == "1"
            ? typeof(NativeHttp.DockerHandlerFactory)
            : typeof(LegacyHttp.DockerHandlerFactory);

        Assert.IsType(expectedFactoryType, actualFactory);
    }

    [Fact]
    public void Build_PreservesClientOptionsAndLogger_WhenSetBeforeTransportSelection()
    {
        var transportFactory = new FakeTransportFactory();

        var transportOptions = new FakeTransportOptions();

        const string headerName = "x-test";
        const string headerValue = "value";

        var apiVersion = new Version(1, 52);
        var endpoint = new Uri("http://localhost:2375");
        var authProvider = new PassThroughAuthProvider(true);
        var timeout = TimeSpan.FromSeconds(1);
        var logger = NullLogger.Instance;

        _ = new DockerClientBuilder()
            .WithApiVersion(apiVersion)
            .WithEndpoint(endpoint)
            .WithAuthProvider(authProvider)
            .WithHeader(headerName, headerValue)
            .WithTimeout(timeout)
            .WithLogger(logger)
            .WithTransportOptions(transportFactory, transportOptions)
            .Build();

        Assert.Equal(apiVersion, transportFactory.LastClientOptions.ApiVersion);
        Assert.Equal(endpoint, transportFactory.LastClientOptions.Endpoint);
        Assert.Same(authProvider, transportFactory.LastClientOptions.AuthProvider);
        Assert.Equal(headerValue, transportFactory.LastClientOptions.Headers[headerName]);
        Assert.Equal(timeout, transportFactory.LastClientOptions.Timeout);
        Assert.Same(logger, transportFactory.LastLogger);
        Assert.Equal(1, transportFactory.TypedCreateHandlerCallCount);
        Assert.Same(transportOptions, transportFactory.LastTransportOptions);
    }

    [Fact]
    public void Build_PreservesClientOptionsAndLogger_WhenSetAfterTransportSelection()
    {
        var transportFactory = new FakeTransportFactory();

        var transportOptions = new FakeTransportOptions();

        const string headerName = "x-test";
        const string headerValue = "value";

        var apiVersion = new Version(1, 52);
        var endpoint = new Uri("http://localhost:2375");
        var authProvider = new PassThroughAuthProvider(true);
        var timeout = TimeSpan.FromSeconds(5);
        var logger = NullLogger.Instance;

        _ = new DockerClientBuilder()
            .WithTransportOptions(transportFactory, transportOptions)
            .WithApiVersion(apiVersion)
            .WithEndpoint(endpoint)
            .WithAuthProvider(authProvider)
            .WithHeader(headerName, headerValue)
            .WithTimeout(timeout)
            .WithLogger(logger)
            .Build();

        Assert.Equal(apiVersion, transportFactory.LastClientOptions.ApiVersion);
        Assert.Equal(endpoint, transportFactory.LastClientOptions.Endpoint);
        Assert.Same(authProvider, transportFactory.LastClientOptions.AuthProvider);
        Assert.Equal(headerValue, transportFactory.LastClientOptions.Headers[headerName]);
        Assert.Equal(timeout, transportFactory.LastClientOptions.Timeout);
        Assert.Same(logger, transportFactory.LastLogger);
        Assert.Equal(1, transportFactory.TypedCreateHandlerCallCount);
        Assert.Same(transportOptions, transportFactory.LastTransportOptions);
    }

    [Fact]
    public void Build_PreservesEndpointInClientOptions_WhenTransportNormalizesEndpoint()
    {
        var transportFactory = new FakeTransportFactory();

        var transportOptions = new FakeTransportOptions();

        var expectedEndpoint = new Uri("npipe://./pipe/docker_engine");

        _ = new DockerClientBuilder()
            .WithTransportOptions(transportFactory, transportOptions)
            .WithEndpoint(expectedEndpoint)
            .Build();

        Assert.Equal(expectedEndpoint, transportFactory.LastClientOptions.Endpoint);
    }

    [Fact]
    public void WithTransportOptions_ReturnsTypedBuilder_WhenBuiltInTransportIsSelected()
    {
        var builder = new DockerClientBuilder();

        Assert.IsType<DockerClientBuilder<LegacyHttpTransportOptions>>(
            builder.WithTransportOptions(new LegacyHttpTransportOptions()));

        Assert.IsType<DockerClientBuilder<NativeHttpTransportOptions>>(
            builder.WithTransportOptions(new NativeHttpTransportOptions()));

        Assert.IsType<DockerClientBuilder<NPipeTransportOptions>>(
            builder.WithTransportOptions(new NPipeTransportOptions()));

        Assert.IsType<DockerClientBuilder<UnixSocketTransportOptions>>(
            builder.WithTransportOptions(new UnixSocketTransportOptions()));
    }

    private sealed class TestDockerClientBuilder : DockerClientBuilder
    {
        public new ClientOptions ClientOptions
            => base.ClientOptions;

        public new ILogger Logger
            => base.Logger;

        public IDockerHandlerFactory ResolveTransportFactory(Uri endpoint)
            => base.ResolveTransportFactory(endpoint.Scheme);
    }

    private sealed class PassThroughAuthProvider(bool tlsEnabled) : IAuthProvider
    {
        public bool TlsEnabled { get; }
            = tlsEnabled;

        public HttpMessageHandler ConfigureHandler(HttpMessageHandler handler)
            => handler;
    }

    private sealed class FakeTransportFactory : IDockerHandlerFactory<FakeTransportOptions>
    {
        public int TypedCreateHandlerCallCount { get; private set; }

        public FakeTransportOptions LastTransportOptions { get; private set; } = null!;

        public ClientOptions LastClientOptions { get; private set; } = null!;

        public ILogger LastLogger { get; private set; } = null!;

        public ResolvedTransport CreateHandler(FakeTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
        {
            TypedCreateHandlerCallCount++;
            LastTransportOptions = transportOptions;
            LastClientOptions = clientOptions;
            LastLogger = logger;

            return new ResolvedTransport(new HttpClientHandler(), clientOptions.Endpoint);
        }

        public ResolvedTransport CreateHandler(ClientOptions clientOptions, ILogger logger)
            => throw new NotSupportedException();

        public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
            => throw new NotSupportedException();
    }

    private sealed record FakeTransportOptions;
}

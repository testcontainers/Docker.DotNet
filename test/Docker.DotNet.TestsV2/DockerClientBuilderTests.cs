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
    public void Build_UsesExpectedFactory_ForSocketSchemes(string endpoint, Type expectedFactoryType)
    {
        var client = new DockerClientBuilder()
            .WithEndpoint(new Uri(endpoint))
            .Build();

        var actualFactory = GetPrivateField<IDockerHandlerFactory>(client, "_handlerFactory");

        Assert.IsType(expectedFactoryType, actualFactory);
    }

    [Fact]
    public void Build_WithHttpEndpoint_UsesConfiguredDefaultHttpFactory()
    {
        var client = new DockerClientBuilder()
            .WithEndpoint(new Uri("http://localhost:2375"))
            .Build();

        var actualFactory = GetPrivateField<IDockerHandlerFactory>(client, "_handlerFactory");

        var expectedFactoryType = Environment.GetEnvironmentVariable("DOCKER_DOTNET_NATIVE_HTTP_ENABLED") == "1"
            ? typeof(NativeHttp.DockerHandlerFactory)
            : typeof(LegacyHttp.DockerHandlerFactory);

        Assert.IsType(expectedFactoryType, actualFactory);
    }

    [Fact]
    public void Build_WithExplicitTransport_UsesProvidedFactoryAndOptions()
    {
        var transportFactory = new FakeTransportFactory();

        var transportOptions = new FakeTransportOptions();

        var client = new DockerClientBuilder()
            .WithTransportOptions(transportFactory, transportOptions)
            .WithEndpoint(new Uri("http://localhost:2375"))
            .WithApiVersion(new Version(1, 52))
            .WithHeader("x-test", "value")
            .WithTimeout(TimeSpan.FromSeconds(1))
            .Build();

        Assert.Equal(1, transportFactory.TypedCreateHandlerCallCount);
        Assert.Same(transportOptions, transportFactory.LastTransportOptions);
        Assert.Equal("http", transportFactory.LastClientOptions.Endpoint.Scheme);
        Assert.Equal("value", transportFactory.LastClientOptions.Headers["x-test"]);
        Assert.Equal(new Version(1, 52), transportFactory.LastClientOptions.ApiVersion);
        Assert.Equal(TimeSpan.FromSeconds(1), transportFactory.LastClientOptions.Timeout);

        var actualFactory = GetPrivateField<IDockerHandlerFactory>(client, "_handlerFactory");
        Assert.Same(transportFactory, actualFactory);
    }

    [Fact]
    public void WithTransportOptions_ReturnsTypedBuilder_ForBuiltInTransports()
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

    private static T GetPrivateField<T>(object instance, string fieldName)
    {
        var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);

        var value = field!.GetValue(instance);
        Assert.NotNull(value);

        return Assert.IsAssignableFrom<T>(value);
    }

    private sealed class TestDockerClientBuilder : DockerClientBuilder
    {
        public new ClientOptions ClientOptions => base.ClientOptions;

        public new ILogger Logger => base.Logger;
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

        public Tuple<HttpMessageHandler, Uri> CreateHandler(FakeTransportOptions transportOptions, ClientOptions clientOptions, ILogger logger)
        {
            TypedCreateHandlerCallCount++;
            LastTransportOptions = transportOptions;
            LastClientOptions = clientOptions;

            return new Tuple<HttpMessageHandler, Uri>(new HttpClientHandler(), new Uri("http://localhost:2375"));
        }


        public Tuple<HttpMessageHandler, Uri> CreateHandler(ClientOptions clientOptions, ILogger logger)
            => throw new NotSupportedException();

        public Task<WriteClosableStream> HijackStreamAsync(HttpContent content)
            => throw new NotSupportedException();
    }

    private sealed record FakeTransportOptions;
}

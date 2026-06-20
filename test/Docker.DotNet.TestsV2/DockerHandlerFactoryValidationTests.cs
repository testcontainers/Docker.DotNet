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
                new Uri("unix:/var/run/docker.sock"),
                "NativeHttpTransportOptions",
                "tcp', 'http', or 'https"
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
}
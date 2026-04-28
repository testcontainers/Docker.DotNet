namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class IDistributionOperationsTests
{
    private readonly TestFixture _testFixture;

    public IDistributionOperationsTests(TestFixture testFixture)
    {
        _testFixture = testFixture;
    }

    [Fact]
    public async Task InspectAsync_ReturnsDescriptorAndPlatforms()
    {
        var response = await _testFixture.DockerClient.Distribution.InspectAsync(
            "alpine:3.20",
            _testFixture.Cts.Token);

        Assert.NotNull(response);
        Assert.NotNull(response.Descriptor);
        Assert.StartsWith("sha256:", response.Descriptor.Digest);
        Assert.NotEmpty(response.Platforms);
    }

    [Fact]
    public Task InspectAsync_UnknownImage_ThrowsDockerImageNotFoundException()
    {
        return Assert.ThrowsAsync<DockerImageNotFoundException>(
            () => _testFixture.DockerClient.Distribution.InspectAsync(
                "alpine:does-not-exist",
                TestContext.Current.CancellationToken));
    }

    [Fact]
    public Task InspectAsync_NullOrEmptyName_ThrowsArgumentNullException()
    {
        return Assert.ThrowsAsync<ArgumentNullException>(
            () => _testFixture.DockerClient.Distribution.InspectAsync(
                string.Empty,
                TestContext.Current.CancellationToken));
    }
}

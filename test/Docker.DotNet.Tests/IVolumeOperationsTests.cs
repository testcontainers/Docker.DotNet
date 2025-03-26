namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class IVolumeOperationsTests
{
    private readonly TestFixture _testFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public IVolumeOperationsTests(TestFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ListAsync_VolumeExists_Succeeds()
    {
        const string volumeName = "docker-dotnet-test-volume";

        await _testFixture.DockerClient.Volumes.CreateAsync(new VolumesCreateParameters
            {
                Name = volumeName,
            },
            _testFixture.Cts.Token);

        try
        {
            var response = await _testFixture.DockerClient.Volumes.ListAsync(new VolumesListParameters
                {
                    Filters = new Dictionary<string, IDictionary<string, bool>>(),
                },
                _testFixture.Cts.Token);

            Assert.Contains(volumeName, response.Volumes.Select(volume => volume.Name));
        }
        finally
        {
            await _testFixture.DockerClient.Volumes.RemoveAsync(volumeName, force: true, _testFixture.Cts.Token);
        }
    }
}
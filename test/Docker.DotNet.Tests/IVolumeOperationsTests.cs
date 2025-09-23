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

    public static IEnumerable<object[]> GetDockerClientTypes() =>
        TestFixture.GetDockerClientTypes();

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task ListAsync_VolumeExists_Succeeds(TestClientsEnum clientType)
    {
        const string volumeName = "docker-dotnet-test-volume";

        await _testFixture.DockerClients[clientType].Volumes.CreateAsync(new VolumesCreateParameters
        {
            Name = volumeName,
        },
            _testFixture.Cts.Token);

        try
        {
            var response = await _testFixture.DockerClients[clientType].Volumes.ListAsync(new VolumesListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>(),
            },
                _testFixture.Cts.Token);

            Assert.Contains(volumeName, response.Volumes.Select(volume => volume.Name));
        }
        finally
        {
            await _testFixture.DockerClients[clientType].Volumes.RemoveAsync(volumeName, force: true, _testFixture.Cts.Token);
        }
    }
}
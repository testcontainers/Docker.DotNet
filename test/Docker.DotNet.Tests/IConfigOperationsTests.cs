namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class IConfigOperationsTests
{
    private readonly TestFixture _testFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public IConfigOperationsTests(TestFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task SwarmConfig_CanCreateAndRead()
    {
        var currentConfigs = await _testFixture.DockerClient.Configs.ListConfigsAsync();

        _testOutputHelper.WriteLine($"Current Configs: {currentConfigs.Count}");

        var testConfigSpec = new SwarmConfigSpec
        {
            Name = $"Config-{Guid.NewGuid().ToString().Substring(1, 10)}",
            Labels = new Dictionary<string, string> { { "key", "value" } },
            Data = new List<byte> { 1, 2, 3, 4, 5 }
        };

        var configParameters = new SwarmCreateConfigParameters
        {
            Config = testConfigSpec
        };

        var createdConfig = await _testFixture.DockerClient.Configs.CreateConfigAsync(configParameters);
        Assert.NotNull(createdConfig.ID);
        _testOutputHelper.WriteLine($"Config created: {createdConfig.ID}");

        var configs = await _testFixture.DockerClient.Configs.ListConfigsAsync();
        Assert.Contains(configs, c => c.ID == createdConfig.ID);
        _testOutputHelper.WriteLine($"Current Configs: {configs.Count}");

        var configResponse = await _testFixture.DockerClient.Configs.InspectConfigAsync(createdConfig.ID);

        Assert.NotNull(configResponse);

        Assert.Equal(configResponse.Spec.Name, testConfigSpec.Name);
        Assert.Equal(configResponse.Spec.Data, testConfigSpec.Data);
        Assert.Equal(configResponse.Spec.Labels, testConfigSpec.Labels);
        Assert.Equal(configResponse.Spec.Templating, testConfigSpec.Templating);


        _testOutputHelper.WriteLine("Config created is the same.");

        await _testFixture.DockerClient.Configs.RemoveConfigAsync(createdConfig.ID);

        await Assert.ThrowsAsync<DockerApiException>(() => _testFixture.DockerClient.Configs.InspectConfigAsync(createdConfig.ID));
    }
}
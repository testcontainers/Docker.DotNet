namespace Docker.DotNet.Tests;

public sealed class ConsoleSizeConverterTests
{
    private const ulong Height = 24;

    private const ulong Width = 80;

    private const string ConsoleSizeArrayFragment = "\"ConsoleSize\":[24,80]";

    private const string ConsoleSizeJson = "{\"ConsoleSize\":[24,80]}";

    [Fact]
    public void Serialize_StandaloneConsoleSize_ProducesJsonObject()
    {
        var consoleSize = new ConsoleSize { Height = Height, Width = Width };

        var jsonString = JsonSerializer.Instance.Serialize(consoleSize);

        Assert.Equal("{\"Height\":24,\"Width\":80}", jsonString);
    }

    [Fact]
    public void Serialize_ContainerExecCreateParameters_WithConsoleSizeProperty_ProducesArrayFormat()
    {
        var parameters = new ContainerExecCreateParameters
        {
            ConsoleSize = new ConsoleSize { Height = Height, Width = Width },
        };

        var jsonString = JsonSerializer.Instance.Serialize(parameters);

        Assert.Contains(ConsoleSizeArrayFragment, jsonString);
    }

    [Fact]
    public void Deserialize_ContainerExecCreateParameters_WithArrayConsoleSize_Succeeds()
    {
        var parameters = JsonSerializer.Instance.Deserialize<ContainerExecCreateParameters>(Encoding.UTF8.GetBytes(ConsoleSizeJson));
        Assert.NotNull(parameters);
        Assert.NotNull(parameters.ConsoleSize);
        Assert.Equal(Height, parameters.ConsoleSize.Height);
        Assert.Equal(Width, parameters.ConsoleSize.Width);
    }

    [Fact]
    public void Serialize_ContainerExecStartParameters_WithConsoleSizeProperty_ProducesArrayFormat()
    {
        var parameters = new ContainerExecStartParameters
        {
            ConsoleSize = new ConsoleSize { Height = Height, Width = Width }
        };

        var jsonString = JsonSerializer.Instance.Serialize(parameters);

        Assert.Contains(ConsoleSizeArrayFragment, jsonString);
    }

    [Fact]
    public void Deserialize_ContainerExecStartParameters_WithArrayConsoleSize_Succeeds()
    {
        var parameters = JsonSerializer.Instance.Deserialize<ContainerExecStartParameters>(Encoding.UTF8.GetBytes(ConsoleSizeJson));
        Assert.NotNull(parameters);
        Assert.NotNull(parameters.ConsoleSize);
        Assert.Equal(Height, parameters.ConsoleSize.Height);
        Assert.Equal(Width, parameters.ConsoleSize.Width);
    }

    [Fact]
    public void Serialize_HostConfig_WithConsoleSizeProperty_ProducesArrayFormat()
    {
        var hostConfig = new HostConfig
        {
            ConsoleSize = new ConsoleSize { Height = Height, Width = Width }
        };

        var jsonString = JsonSerializer.Instance.Serialize(hostConfig);

        Assert.Contains(ConsoleSizeArrayFragment, jsonString);
    }

    [Fact]
    public void Deserialize_HostConfig_WithArrayConsoleSize_Succeeds()
    {
        var hostConfig = JsonSerializer.Instance.Deserialize<HostConfig>(Encoding.UTF8.GetBytes(ConsoleSizeJson));
        Assert.NotNull(hostConfig);
        Assert.NotNull(hostConfig.ConsoleSize);
        Assert.Equal(Height, hostConfig.ConsoleSize.Height);
        Assert.Equal(Width, hostConfig.ConsoleSize.Width);
    }

    [Theory]
    [InlineData("{\"ConsoleSize\":[24]}")]
    [InlineData("{\"ConsoleSize\":[24,80,1]}")]
    [InlineData("{\"ConsoleSize\":[\"24\",80]}")]
    public void Deserialize_ContainerExecCreateParameters_WithInvalidArrayConsoleSize_ThrowsJsonException(string jsonString)
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Instance.Deserialize<ContainerExecCreateParameters>(Encoding.UTF8.GetBytes(jsonString)));
    }
}
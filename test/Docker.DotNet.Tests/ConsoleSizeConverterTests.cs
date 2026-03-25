namespace Docker.DotNet.Tests;

public sealed class ConsoleSizeConverterTests
{
    private const ulong Height = 24;
    private const ulong Width = 80;

    [Fact]
    public void Serialize_ConsoleSize_ProducesJsonObject()
    {
        var consoleSize = new ConsoleSize { Height = Height, Width = Width };

        var jsonString = JsonSerializer.Instance.Serialize(consoleSize);

        Assert.Equal("{\"Height\":24,\"Width\":80}", jsonString);
    }

    [Fact]
    public void Serialize_ContainerExecCreateParameters_WithConsoleSize_ProducesArrayFormat()
    {
        var parameters = new ContainerExecCreateParameters
        {
            ConsoleSize = new ConsoleSize { Height = Height, Width = Width },
            AttachStdin = true,
            AttachStdout = true,
            TTY = true,
            Cmd = new List<string> { "/bin/bash" }
        };

        var jsonString = JsonSerializer.Instance.Serialize(parameters);

        Assert.Contains("\"ConsoleSize\":[24,80]", jsonString);
    }

    [Fact]
    public void Deserialize_ContainerExecCreateParameters_WithArrayConsoleSize_Succeeds()
    {
        var json = "{\"ConsoleSize\":[24,80],\"AttachStdin\":true,\"Tty\":true,\"Cmd\":[\"/bin/bash\"]}";

        var parameters = JsonSerializer.Instance.Deserialize<ContainerExecCreateParameters>(Encoding.UTF8.GetBytes(json));

        Assert.NotNull(parameters);
        Assert.NotNull(parameters.ConsoleSize);
        Assert.Equal(Height, parameters.ConsoleSize.Height);
        Assert.Equal(Width, parameters.ConsoleSize.Width);
    }

    [Fact]
    public void Serialize_ContainerExecStartParameters_WithConsoleSize_ProducesArrayFormat()
    {
        var parameters = new ContainerExecStartParameters
        {
            ConsoleSize = new ConsoleSize { Height = Height, Width = Width }
        };

        var jsonString = JsonSerializer.Instance.Serialize(parameters);

        Assert.Contains("\"ConsoleSize\":[24,80]", jsonString);
    }

    [Fact]
    public void Deserialize_ContainerExecStartParameters_WithArrayConsoleSize_Succeeds()
    {
        var json = "{\"ConsoleSize\":[24,80]}";

        var parameters = JsonSerializer.Instance.Deserialize<ContainerExecStartParameters>(Encoding.UTF8.GetBytes(json));

        Assert.NotNull(parameters);
        Assert.NotNull(parameters.ConsoleSize);
        Assert.Equal(Height, parameters.ConsoleSize.Height);
        Assert.Equal(Width, parameters.ConsoleSize.Width);
    }

    [Fact]
    public void Serialize_HostConfig_WithConsoleSize_ProducesArrayFormat()
    {
        var hostConfig = new HostConfig
        {
            ConsoleSize = new ConsoleSize { Height = Height, Width = Width }
        };

        var jsonString = JsonSerializer.Instance.Serialize(hostConfig);

        Assert.Contains("\"ConsoleSize\":[24,80]", jsonString);
    }

    [Fact]
    public void Deserialize_HostConfig_WithArrayConsoleSize_Succeeds()
    {
        var json = "{\"ConsoleSize\":[24,80]}";

        var hostConfig = JsonSerializer.Instance.Deserialize<HostConfig>(Encoding.UTF8.GetBytes(json));

        Assert.NotNull(hostConfig);
        Assert.NotNull(hostConfig.ConsoleSize);
        Assert.Equal(Height, hostConfig.ConsoleSize.Height);
        Assert.Equal(Width, hostConfig.ConsoleSize.Width);
    }
}

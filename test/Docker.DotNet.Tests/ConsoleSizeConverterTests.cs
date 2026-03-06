namespace Docker.DotNet.Tests;

public sealed class ConsoleSizeConverterTests
{
    [Fact]
    public void Serialize_ConsoleSize_ProducesJsonArray()
    {
        // Given
        var consoleSize = new ConsoleSize { Height = 24, Width = 80 };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(consoleSize);

        // Then
        Assert.Equal("[24,80]", jsonString);
    }

    [Fact]
    public void Deserialize_JsonArray_ProducesConsoleSize()
    {
        // Given
        var json = "[24,80]";

        // When
        var consoleSize = JsonSerializer.Instance.Deserialize<ConsoleSize>(Encoding.UTF8.GetBytes(json));

        // Then
        Assert.NotNull(consoleSize);
        Assert.Equal(24UL, consoleSize.Height);
        Assert.Equal(80UL, consoleSize.Width);
    }

    [Fact]
    public void SerializeAndDeserialize_RoundTrip_Succeeds()
    {
        // Given
        var original = new ConsoleSize { Height = 50, Width = 200 };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(original);
        var deserialized = JsonSerializer.Instance.Deserialize<ConsoleSize>(Encoding.UTF8.GetBytes(jsonString));

        // Then
        Assert.NotNull(deserialized);
        Assert.Equal(original.Height, deserialized.Height);
        Assert.Equal(original.Width, deserialized.Width);
    }

    [Fact]
    public void Serialize_ContainerExecCreateParameters_WithConsoleSize_ProducesArrayFormat()
    {
        // Given
        var parameters = new ContainerExecCreateParameters
        {
            ConsoleSize = new ConsoleSize { Height = 24, Width = 80 },
            AttachStdin = true,
            AttachStdout = true,
            TTY = true,
            Cmd = new List<string> { "/bin/bash" }
        };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(parameters);

        // Then - ConsoleSize should be serialized as [24,80], not {"Height":24,"Width":80}
        Assert.Contains("\"ConsoleSize\":[24,80]", jsonString);
        Assert.DoesNotContain("\"Height\"", jsonString);
        Assert.DoesNotContain("\"Width\"", jsonString);
    }

    [Fact]
    public void Serialize_ContainerExecCreateParameters_WithoutConsoleSize_OmitsField()
    {
        // Given
        var parameters = new ContainerExecCreateParameters
        {
            AttachStdin = true,
            AttachStdout = true,
            TTY = true,
            Cmd = new List<string> { "/bin/bash" }
        };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(parameters);

        // Then
        Assert.DoesNotContain("ConsoleSize", jsonString);
    }

    [Fact]
    public void Serialize_ContainerExecStartParameters_WithConsoleSize_ProducesArrayFormat()
    {
        // Given
        var parameters = new ContainerExecStartParameters
        {
            ConsoleSize = new ConsoleSize { Height = 30, Width = 120 }
        };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(parameters);

        // Then
        Assert.Contains("\"ConsoleSize\":[30,120]", jsonString);
    }

    [Fact]
    public void Deserialize_ContainerExecCreateParameters_WithArrayConsoleSize_Succeeds()
    {
        // Given - This is the format Docker API would return
        var json = "{\"ConsoleSize\":[24,80],\"AttachStdin\":true,\"Tty\":true,\"Cmd\":[\"/bin/bash\"]}";

        // When
        var parameters = JsonSerializer.Instance.Deserialize<ContainerExecCreateParameters>(Encoding.UTF8.GetBytes(json));

        // Then
        Assert.NotNull(parameters);
        Assert.NotNull(parameters.ConsoleSize);
        Assert.Equal(24UL, parameters.ConsoleSize.Height);
        Assert.Equal(80UL, parameters.ConsoleSize.Width);
    }

    [Fact]
    public void Serialize_LargeConsoleSize_HandlesCorrectly()
    {
        // Given - Test with larger values that still fit in ulong
        var consoleSize = new ConsoleSize { Height = 1000, Width = 2000 };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(consoleSize);

        // Then
        Assert.Equal("[1000,2000]", jsonString);
    }

    [Fact]
    public void Serialize_ZeroConsoleSize_HandlesCorrectly()
    {
        // Given
        var consoleSize = new ConsoleSize { Height = 0, Width = 0 };

        // When
        var jsonString = JsonSerializer.Instance.Serialize(consoleSize);

        // Then
        Assert.Equal("[0,0]", jsonString);
    }
}

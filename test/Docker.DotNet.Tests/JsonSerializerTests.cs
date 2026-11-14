namespace Docker.DotNet.Tests;

public sealed class JsonSerializerTests
{
    [Fact]
    public void Deserialize_ShouldIgnoreGlobalRespectNullableAnnotationsSwitch()
    {
        // Given
        var json = Encoding.UTF8.GetBytes("""{"Name":null}""");

        // When
        var volume = JsonSerializer.Instance.Deserialize<VolumeResponse>(json);

        // Then
        Assert.Null(volume.Name);
    }

    [Fact]
    public void Deserialize_ShouldIgnoreGlobalRespectRequiredConstructorParametersSwitch()
    {
        // Given
        var json = Encoding.UTF8.GetBytes("{}");

        // When
        var contextMeta = JsonSerializer.Instance.Deserialize<DockerConfig.DockerContextMeta>(json);

        // Then
        Assert.Null(contextMeta.Endpoints);
    }
}

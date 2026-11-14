namespace Docker.DotNet.Tests.StrictJson;

public sealed class JsonSerializerTests
{
    [Fact]
    public void Deserialize_ShouldRespectNullableAnnotations()
    {
        // Given
        var json = Encoding.UTF8.GetBytes("""{"Name":null}""");

        // When
        var exception = Record.Exception(() => JsonSerializer.Instance.Deserialize<VolumeResponse>(json));

        // Then
        AssertStrictDeserialization(exception);
    }

    [Fact]
    public void Deserialize_ShouldRespectRequiredConstructorParameters()
    {
        // Given
        var json = Encoding.UTF8.GetBytes("{}");

        // When
        var exception = Record.Exception(() => JsonSerializer.Instance.Deserialize<DockerConfig.DockerContextMeta>(json));

        // Then
        AssertStrictDeserialization(exception);
    }

    private static void AssertStrictDeserialization(Exception exception)
    {
#if NET9_0_OR_GREATER
        Assert.IsType<JsonException>(exception);
#else
        // System.Text.Json 8 does not support strict deserialization, so the
        // switches have no effect.
        Assert.Null(exception);
#endif
    }
}

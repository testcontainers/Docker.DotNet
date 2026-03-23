namespace Docker.DotNet.Tests;

public sealed class JsonRequestContentTests
{
    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenValueIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new JsonRequestContent<object>(null, JsonSerializer.Instance));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenSerializerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new JsonRequestContent<object>(new object(), null));
    }

    [Fact]
    public async Task GetContent_Succeeds_WhenValueAndSerializerAreValid()
    {
        var content = new JsonRequestContent<Dictionary<string, string>[]>([new Dictionary<string, string>() { { "key", "value" } }], JsonSerializer.Instance);
        using var httpContent = content.GetContent();
        Assert.Equal("application/json; charset=utf-8", httpContent.Headers.ContentType.ToString());
        var jsonString = await httpContent.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.Equal("""[{"key":"value"}]""", jsonString);
    }
}
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
        var content = new JsonRequestContent<int[]>(new[] { 1 }, JsonSerializer.Instance);
        using var httpContent = content.GetContent();
        var jsonString = await httpContent.ReadAsStringAsync();
        Assert.Equal("[1]", jsonString);
    }
}
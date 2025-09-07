using System.Net.Http;

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
        Assert.Throws<ArgumentNullException>(() => new JsonRequestContent<object>(new(), null));
    }

    [Fact]
    public async Task GetContent_Succeeds_WhenValueAndSerializerAreValid()
    {
        JsonRequestContent<int[]> content = new([1], JsonSerializer.Instance);
        using HttpContent httpContent = content.GetContent();
        string jsonString = await httpContent.ReadAsStringAsync();
        Assert.Equal("[1]", jsonString);
    }
}

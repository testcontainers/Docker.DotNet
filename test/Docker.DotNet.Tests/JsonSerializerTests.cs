using System.IO;

namespace Docker.DotNet.Tests;

public class JsonSerializerTests
{
    [Fact]
    public async Task DeserializeAsync_CleanJson_ParsesSuccessfully()
    {
        // Arrange: Clean JSON without null bytes (regression test)
        var json = """{"status":"Pulling from library/alpine"}{"status":"Downloading"}""";
        var bytes = Encoding.UTF8.GetBytes(json);
        using var stream = new MemoryStream(bytes);

        // Act
        var results = new List<TestMessage>();
        await foreach (var message in JsonSerializer.Instance.DeserializeAsync<TestMessage>(stream, CancellationToken.None))
        {
            results.Add(message);
        }

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal("Pulling from library/alpine", results[0].Status);
        Assert.Equal("Downloading", results[1].Status);
    }

    [Fact]
    public async Task DeserializeAsync_LeadingNullBytes_ParsesSuccessfully()
    {
        // Arrange: Leading null bytes before JSON
        var json = """{"status":"Test"}""";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var bytes = new byte[5 + jsonBytes.Length];
        // First 5 bytes are null (0x00)
        Array.Copy(jsonBytes, 0, bytes, 5, jsonBytes.Length);
        using var stream = new MemoryStream(bytes);

        // Act
        var results = new List<TestMessage>();
        await foreach (var message in JsonSerializer.Instance.DeserializeAsync<TestMessage>(stream, CancellationToken.None))
        {
            results.Add(message);
        }

        // Assert
        Assert.Single(results);
        Assert.Equal("Test", results[0].Status);
    }

    [Fact]
    public async Task DeserializeAsync_NullBytesBetweenJsonDocuments_ParsesSuccessfully()
    {
        // Arrange: JSON documents separated by null bytes (Docker 29.x behavior)
        var json1 = """{"status":"First"}""";
        var json2 = """{"status":"Second"}""";
        var json1Bytes = Encoding.UTF8.GetBytes(json1);
        var json2Bytes = Encoding.UTF8.GetBytes(json2);

        var bytes = new byte[json1Bytes.Length + 3 + json2Bytes.Length];
        Array.Copy(json1Bytes, 0, bytes, 0, json1Bytes.Length);
        // 3 null bytes between documents
        Array.Copy(json2Bytes, 0, bytes, json1Bytes.Length + 3, json2Bytes.Length);
        using var stream = new MemoryStream(bytes);

        // Act
        var results = new List<TestMessage>();
        await foreach (var message in JsonSerializer.Instance.DeserializeAsync<TestMessage>(stream, CancellationToken.None))
        {
            results.Add(message);
        }

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal("First", results[0].Status);
        Assert.Equal("Second", results[1].Status);
    }

    [Fact]
    public async Task DeserializeAsync_OnlyNullBytes_ReturnsEmpty()
    {
        // Arrange: Stream containing only null bytes
        var bytes = new byte[10]; // All zeros
        using var stream = new MemoryStream(bytes);

        // Act
        var results = new List<TestMessage>();
        await foreach (var message in JsonSerializer.Instance.DeserializeAsync<TestMessage>(stream, CancellationToken.None))
        {
            results.Add(message);
        }

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public async Task DeserializeAsync_MultipleDocumentsWithVaryingNullBytePadding_ParsesSuccessfully()
    {
        // Arrange: Multiple JSON documents with varying null byte padding
        var messages = new[] { "First", "Second", "Third", "Fourth" };
        var nullPaddings = new[] { 0, 1, 5, 2 }; // Varying padding before each message

        using var memoryStream = new MemoryStream();
        for (int i = 0; i < messages.Length; i++)
        {
            // Add null bytes padding
            memoryStream.Write(new byte[nullPaddings[i]]);

            // Add JSON document
            var json = $@"{{""status"":""{messages[i]}""}}";
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            memoryStream.Write(jsonBytes);
        }

        memoryStream.Position = 0;

        // Act
        var results = new List<TestMessage>();
        await foreach (var message in JsonSerializer.Instance.DeserializeAsync<TestMessage>(memoryStream, CancellationToken.None))
        {
            results.Add(message);
        }

        // Assert
        Assert.Equal(4, results.Count);
        for (int i = 0; i < messages.Length; i++)
        {
            Assert.Equal(messages[i], results[i].Status);
        }
    }

    [Fact]
    public async Task DeserializeAsync_TrailingNullBytes_ParsesSuccessfully()
    {
        // Arrange: JSON followed by trailing null bytes
        var json = """{"status":"Test"}""";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var bytes = new byte[jsonBytes.Length + 10];
        Array.Copy(jsonBytes, 0, bytes, 0, jsonBytes.Length);
        // Last 10 bytes are null (0x00)
        using var stream = new MemoryStream(bytes);

        // Act
        var results = new List<TestMessage>();
        await foreach (var message in JsonSerializer.Instance.DeserializeAsync<TestMessage>(stream, CancellationToken.None))
        {
            results.Add(message);
        }

        // Assert
        Assert.Single(results);
        Assert.Equal("Test", results[0].Status);
    }

    [Fact]
    public async Task DeserializeAsync_EmptyStream_ReturnsEmpty()
    {
        // Arrange: Empty stream
        using var stream = new MemoryStream();

        // Act
        var results = new List<TestMessage>();
        await foreach (var message in JsonSerializer.Instance.DeserializeAsync<TestMessage>(stream, CancellationToken.None))
        {
            results.Add(message);
        }

        // Assert
        Assert.Empty(results);
    }

    private class TestMessage
    {
        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public string Status { get; set; }
    }
}

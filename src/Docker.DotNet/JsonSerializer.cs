namespace Docker.DotNet;

internal sealed class JsonSerializer
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

    static JsonSerializer()
    {
    }

    private JsonSerializer()
    {
        _options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        _options.Converters.Add(new JsonEnumMemberConverter<RestartPolicyKind>());
        _options.Converters.Add(new JsonEnumMemberConverter<TaskState>());
        _options.Converters.Add(new JsonDateTimeConverter());
        _options.Converters.Add(new JsonNullableDateTimeConverter());
    }

    public static JsonSerializer Instance { get; }
        = new JsonSerializer();

    public HttpContent GetHttpContent<T>(T value)
    {
        return new StringContent(Serialize(value), Encoding.UTF8, "application/json");
    }

    public string Serialize<T>(T value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, _options);
    }

    public byte[] SerializeToUtf8Bytes<T>(T value)
    {
        return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value, _options);
    }

    public T Deserialize<T>(byte[] json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(json, _options);
    }

    public Task<T> DeserializeAsync<T>(HttpContent content, CancellationToken cancellationToken)
    {
        return content.ReadFromJsonAsync<T>(_options, cancellationToken);
    }

    public async IAsyncEnumerable<T> DeserializeAsync<T>(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var reader = PipeReader.Create(stream);

        while (true)
        {
            var result = await reader.ReadAsync(cancellationToken)
                .ConfigureAwait(false);

            var buffer = result.Buffer;

            while (!buffer.IsEmpty && TryParseJson(ref buffer, out var jsonDocument))
            {
                yield return jsonDocument.Deserialize<T>(_options);
            }

            if (result.IsCompleted)
            {
                break;
            }

            reader.AdvanceTo(buffer.Start, buffer.End);
        }

        await reader.CompleteAsync();
    }

    private static bool TryParseJson(ref ReadOnlySequence<byte> buffer, out JsonDocument jsonDocument)
    {
        // Skip leading null bytes (Docker 29.x can include them between JSON documents)
        buffer = SkipLeadingNullBytes(buffer);

        if (buffer.IsEmpty)
        {
            jsonDocument = null;
            return false;
        }

        var reader = new Utf8JsonReader(buffer, isFinalBlock: false, default);

        if (JsonDocument.TryParseValue(ref reader, out jsonDocument))
        {
            buffer = buffer.Slice(reader.BytesConsumed);
            return true;
        }

        jsonDocument = null;
        return false;
    }

    private static ReadOnlySequence<byte> SkipLeadingNullBytes(ReadOnlySequence<byte> buffer)
    {
        long nullByteCount = 0;

        foreach (var segment in buffer)
        {
            var span = segment.Span;
            var idx = span.IndexOfAnyExcept((byte)0);
            if (idx >= 0)
            {
                nullByteCount += idx;
                return nullByteCount > 0 ? buffer.Slice(nullByteCount) : buffer;
            }
            nullByteCount += span.Length;
        }

        // All bytes are null - return empty slice from the original buffer to preserve position compatibility
        return buffer.Slice(buffer.Length);
    }
}
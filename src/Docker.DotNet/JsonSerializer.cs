namespace Docker.DotNet;

internal sealed class JsonSerializer
{
    private static readonly MediaTypeHeaderValue ContentTypeApplicationJsonHeader = new("application/json") { CharSet = Encoding.UTF8.WebName };

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
        var content = new ByteArrayContent(SerializeToUtf8Bytes(value));
        content.Headers.ContentType = ContentTypeApplicationJsonHeader;
        return content;
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
        var reader = new Utf8JsonReader(buffer, isFinalBlock: false, default);

        if (JsonDocument.TryParseValue(ref reader, out jsonDocument))
        {
            buffer = buffer.Slice(reader.BytesConsumed);
            return true;
        }

        return false;
    }
}
namespace Docker.DotNet;

internal sealed class JsonSerializer
{
    private static readonly MediaTypeHeaderValue ApplicationJsonUtf8ContentType = new MediaTypeHeaderValue("application/json")
    {
        CharSet = Encoding.UTF8.WebName
    };

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

    static JsonSerializer()
    {
    }

    private JsonSerializer()
    {
        _options.TypeInfoResolver = JsonTypeInfoResolver.Combine(
            DockerModelsJsonSerializerContext.Default,
            DockerExtendedJsonSerializerContext.Default);
        _options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        _options.Converters.Add(new JsonEnumMemberConverter<RestartPolicyKind>());
        _options.Converters.Add(new JsonEnumMemberConverter<TaskState>());
        _options.Converters.Add(new JsonDateTimeConverter());
        _options.Converters.Add(new JsonNullableDateTimeConverter());
        _options.MakeReadOnly();
    }

    public static JsonSerializer Instance { get; }
        = new JsonSerializer();

    public HttpContent GetHttpContent<T>(T value)
    {
        return new ByteArrayContent(SerializeToUtf8Bytes(value))
        {
            Headers =
            {
                ContentType = ApplicationJsonUtf8ContentType
            }
        };
    }

    public string Serialize<T>(T value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, JsonTypeInfoCache<T>.Value);
    }

    public byte[] SerializeToUtf8Bytes<T>(T value)
    {
        return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value, JsonTypeInfoCache<T>.Value);
    }

    public T Deserialize<T>(byte[] json)
    {
        return System.Text.Json.JsonSerializer.Deserialize(json, JsonTypeInfoCache<T>.Value)!;
    }

    public Task<T> DeserializeAsync<T>(HttpContent content, CancellationToken cancellationToken)
    {
        return content.ReadFromJsonAsync(JsonTypeInfoCache<T>.Value, cancellationToken)!;
    }

    public async IAsyncEnumerable<T> DeserializeAsync<T>(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var jsonTypeInfo = JsonTypeInfoCache<T>.Value;
        var reader = PipeReader.Create(stream);

        while (true)
        {
            var result = await reader.ReadAsync(cancellationToken)
                .ConfigureAwait(false);

            var buffer = result.Buffer;

            while (!buffer.IsEmpty && TryParseJson(ref buffer, out var jsonDocument))
            {
                yield return jsonDocument!.Deserialize(jsonTypeInfo)!;
            }

            if (result.IsCompleted)
            {
                break;
            }

            reader.AdvanceTo(buffer.Start, buffer.End);
        }

        await reader.CompleteAsync();
    }

    private static bool TryParseJson(ref ReadOnlySequence<byte> buffer, out JsonDocument? jsonDocument)
    {
        var reader = new Utf8JsonReader(buffer, isFinalBlock: false, default);

        if (JsonDocument.TryParseValue(ref reader, out jsonDocument))
        {
            buffer = buffer.Slice(reader.BytesConsumed);
            return true;
        }

        return false;
    }

    private static class JsonTypeInfoCache<T>
    {
        public static readonly JsonTypeInfo<T> Value = (JsonTypeInfo<T>)Instance._options.GetTypeInfo(typeof(T));
    }
}

// Additional source-generated metadata for collections and dictionaries used by operations.

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(IList<string>))]

// Filters
[JsonSerializable(typeof(IDictionary<string, IDictionary<string, bool>>))]
[JsonSerializable(typeof(IDictionary<string, string>))]

// ConfigOperations.ListConfigsAsync
[JsonSerializable(typeof(SwarmConfig[]))]

// ContainerOperations.ListContainersAsync
[JsonSerializable(typeof(ContainerListResponse[]))]
// ContainerOperations.InspectChangesAsync
[JsonSerializable(typeof(ContainerFileSystemChangeResponse[]))]

// ImageOperations.ListImagesAsync
[JsonSerializable(typeof(ImagesListResponse[]))]
// ImageOperations.GetImageHistoryAsync
[JsonSerializable(typeof(ImageHistoryResponse[]))]
// ImageOperations.DeleteImageAsync
[JsonSerializable(typeof(Dictionary<string, string>[]))]
// ImageOperations.SearchImagesAsync
[JsonSerializable(typeof(ImageSearchResponse[]))]
// ImageOperations.RegistryConfigHeaders
[JsonSerializable(typeof(Dictionary<string, AuthConfig>))]

// NetworkOperations.ListNetworksAsync
[JsonSerializable(typeof(NetworkResponse[]))]

// PluginOperations.ListPluginsAsync
[JsonSerializable(typeof(Plugin[]))]
// PluginOperations.GetPrivilegesAsync
[JsonSerializable(typeof(PluginPrivilege[]))]
// PluginOperations.InstallPluginAsync
[JsonSerializable(typeof(IList<PluginPrivilege>))]

// SecretOperations.ListAsync
[JsonSerializable(typeof(Secret[]))]

// SwarmOperations.ListServicesAsync
[JsonSerializable(typeof(SwarmService[]))]
// SwarmOperations.ListNodesAsync
[JsonSerializable(typeof(NodeListResponse[]))]

// TaskOperations.ListAsync
[JsonSerializable(typeof(TaskResponse[]))]
internal sealed partial class DockerExtendedJsonSerializerContext : JsonSerializerContext { }

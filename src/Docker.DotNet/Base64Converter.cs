namespace Docker.DotNet;

internal class Base64Converter : JsonConverter<IList<byte>>
{
    public override IList<byte> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var base64 = reader.GetString();
        return base64 == null ? null : Encoding.UTF8.GetBytes(base64);
    }

    public override void Write(Utf8JsonWriter writer, IList<byte> value, JsonSerializerOptions options)
    {
        // TODO: This seams not correct, it breaks: 'SwarmConfig_CanCreateAndRead'.
        var base64 = Encoding.UTF8.GetString(value.ToArray());
        writer.WriteStringValue(base64);
    }
}
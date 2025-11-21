namespace Docker.DotNet;

internal class Base64Converter : JsonConverter<IList<byte>>
{
    public override IList<byte> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var base64String = reader.GetString();
        return base64String == null ? null : Convert.FromBase64String(base64String);
    }

    public override void Write(Utf8JsonWriter writer, IList<byte> value, JsonSerializerOptions options)
    {
        var base64String = Convert.ToBase64String(value.ToArray());
        writer.WriteStringValue(base64String);
    }
}
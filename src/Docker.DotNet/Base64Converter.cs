namespace Docker.DotNet;

internal class Base64Converter : JsonConverter<IList<byte>>
{
    public override IList<byte> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetBytesFromBase64();
    }

    public override void Write(Utf8JsonWriter writer, IList<byte> value, JsonSerializerOptions options)
    {
        writer.WriteBase64StringValue(value.ToArray());
    }
}
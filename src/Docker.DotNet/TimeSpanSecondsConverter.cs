namespace Docker.DotNet;

internal sealed class TimeSpanSecondsConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeSpan.FromSeconds(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue((long)value.TotalSeconds);
    }
}
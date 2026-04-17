namespace Docker.DotNet;

internal sealed class TimeSpanNanosecondsConverter : JsonConverter<TimeSpan>
{
    private const int NanosecondsPerTick = 100;

    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueInNanoseconds = reader.GetInt64();
        return TimeSpan.FromTicks(valueInNanoseconds / NanosecondsPerTick);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan timeSpan, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(timeSpan.Ticks * NanosecondsPerTick);
    }
}
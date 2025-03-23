namespace Docker.DotNet;

internal class TimeSpanNanosecondsConverter : JsonConverter<TimeSpan>
{
    private const int MilliSecondToNanoSecond = 1000000;

    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueInNanoSeconds = reader.GetInt64();
        var milliSecondValue = valueInNanoSeconds / MilliSecondToNanoSecond;
        return TimeSpan.FromMilliseconds(milliSecondValue);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan timeSpan, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(timeSpan.TotalMilliseconds * MilliSecondToNanoSecond);
    }
}
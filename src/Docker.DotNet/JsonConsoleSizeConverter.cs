namespace Docker.DotNet;

internal sealed class JsonConsoleSizeConverter : JsonConverter<ConsoleSize?>
{
    public override ConsoleSize? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected a JSON array for ConsoleSize.");
        }

        var height = ReadArrayElement(ref reader, "height");

        var width = ReadArrayElement(ref reader, "width");

        if (!reader.Read())
        {
            throw new JsonException("Expected the ConsoleSize array to end after two numeric elements.");
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Expected the ConsoleSize array to contain exactly two numeric elements.");
        }

        return new ConsoleSize
        {
            Height = height,
            Width = width
        };
    }

    private static ulong ReadArrayElement(ref Utf8JsonReader reader, string elementName)
    {
        if (!reader.Read())
        {
            throw new JsonException($"Expected a numeric '{elementName}' element in the ConsoleSize array.");
        }

        if (reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException($"Expected the ConsoleSize '{elementName}' element to be a number.");
        }

        return reader.GetUInt64();
    }

    public override void Write(Utf8JsonWriter writer, ConsoleSize? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.Height);
        writer.WriteNumberValue(value.Width);
        writer.WriteEndArray();
    }
}
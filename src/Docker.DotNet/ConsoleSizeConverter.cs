using Docker.DotNet.Models;

namespace Docker.DotNet;

/// <summary>
/// Serializes <see cref="ConsoleSize"/> as a JSON array [height, width] to match
/// the Docker Engine API's Go type <c>[2]uint</c>.
/// </summary>
internal class ConsoleSizeConverter : JsonConverter<ConsoleSize>
{
    public override ConsoleSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected a JSON array for ConsoleSize.");
        }

        reader.Read();
        var height = reader.GetUInt64();

        reader.Read();
        var width = reader.GetUInt64();

        reader.Read(); // EndArray

        return new ConsoleSize
        {
            Height = height,
            Width = width
        };
    }

    public override void Write(Utf8JsonWriter writer, ConsoleSize value, JsonSerializerOptions options)
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

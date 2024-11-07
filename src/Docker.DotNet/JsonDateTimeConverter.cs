namespace Docker.DotNet;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class JsonDateTimeConverter : JsonConverter<DateTime>
{
    private static readonly DateTime UnixEpochBase = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return UnixEpochBase.AddSeconds(reader.GetInt64());
            case JsonTokenType.String:
                return reader.GetDateTime();
            default:
                throw new NotImplementedException($"Deserializing JSON '{reader.TokenType}' to DateTime is not handled.");
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
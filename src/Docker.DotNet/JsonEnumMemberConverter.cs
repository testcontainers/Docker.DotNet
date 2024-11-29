namespace Docker.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class JsonEnumMemberConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    private readonly IReadOnlyDictionary<string, string> _map = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(field => (Name: field.Name, Attribute: field.GetCustomAttribute<EnumMemberAttribute>()))
        .Where(item => item.Attribute != null && item.Attribute.Value != null)
        .ToDictionary(item => item.Name, item => item.Attribute.Value, StringComparer.OrdinalIgnoreCase);

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();

        var enumValue = _map.SingleOrDefault(item => item.Value.Equals(stringValue, StringComparison.OrdinalIgnoreCase));

        if (enumValue.Key == null)
        {
            throw new JsonException($"Unknown enum value '{stringValue}' for enum type '{typeof(TEnum).Name}'.");
        }

        if (!Enum.TryParse(enumValue.Key, out TEnum result))
        {
            throw new JsonException($"Unable to convert '{stringValue}' to a valid enum value of type '{typeof(TEnum).Name}'.");
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var enumName = value.ToString();
        writer.WriteStringValue(_map.TryGetValue(enumName, out var stringValue) ? stringValue : enumName);
    }
}
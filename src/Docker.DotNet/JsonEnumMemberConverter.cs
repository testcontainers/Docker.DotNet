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
    private readonly Dictionary<string, string> _enumFields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(field => (Name: field.Name, Attribute: field.GetCustomAttribute<EnumMemberAttribute>()))
        .Where(item => item.Attribute != null && item.Attribute.Value != null)
        .ToDictionary(item => item.Name, item => item.Attribute.Value);

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();

        var enumField = _enumFields.SingleOrDefault(item => item.Value.Equals(stringValue, StringComparison.Ordinal));

        if (enumField.Key == null)
        {
            throw new JsonException($"Unknown enum value '{stringValue}' for enum type '{typeof(TEnum).Name}'.");
        }

        if (!Enum.TryParse(enumField.Key, out TEnum enumValue))
        {
            throw new JsonException($"Unable to convert '{stringValue}' to a valid enum value of type '{typeof(TEnum).Name}'.");
        }

        return enumValue;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        var enumName = value.ToString();

        if (!_enumFields.TryGetValue(enumName, out var stringValue))
        {
            throw new JsonException($"Unable to convert '{enumName}' to a valid enum value of type '{nameof(String)}'.");
        }

        writer.WriteStringValue(stringValue);
    }
}
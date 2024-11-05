namespace Docker.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

// https://github.com/dotnet/runtime/issues/74385#issuecomment-1705083109.
internal sealed class JsonEnumMemberConverter<TEnum> : JsonStringEnumConverter<TEnum> where TEnum : struct, Enum
{
    public JsonEnumMemberConverter() : base(ResolveNamingPolicy())
    {
    }

    private static JsonNamingPolicy ResolveNamingPolicy()
    {
        return new EnumMemberNamingPolicy(typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(fieldInfo => new KeyValuePair<string, string>(fieldInfo.Name, fieldInfo.GetCustomAttribute<EnumMemberAttribute>()?.Value))
            .Where(kvp => kvp.Value != null)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
    }

    private sealed class EnumMemberNamingPolicy : JsonNamingPolicy
    {
        private readonly IReadOnlyDictionary<string, string> _map;

        public EnumMemberNamingPolicy(IReadOnlyDictionary<string, string> map) => _map = map;

        public override string ConvertName(string name) => _map.TryGetValue(name, out var newName) ? newName : name;
    }
}
#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginEnv // (plugin.Env)
    {
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Settable")]
        public IList<string> Settable { get; set; } = default!;

        [JsonPropertyName("Value")]
        public string? Value { get; set; }
    }
}

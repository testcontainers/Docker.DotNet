#nullable enable
namespace Docker.DotNet.Models
{
    public class Plugin // (plugin.Plugin)
    {
        [JsonPropertyName("Config")]
        public PluginConfig Config { get; set; } = default!;

        [JsonPropertyName("Enabled")]
        public bool Enabled { get; set; } = default!;

        [JsonPropertyName("Id")]
        public string? ID { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("PluginReference")]
        public string? PluginReference { get; set; }

        [JsonPropertyName("Settings")]
        public PluginSettings Settings { get; set; } = default!;
    }
}

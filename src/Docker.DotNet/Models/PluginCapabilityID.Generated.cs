#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginCapabilityID // (plugin.CapabilityID)
    {
        [JsonPropertyName("Capability")]
        public string Capability { get; set; } = default!;

        [JsonPropertyName("Prefix")]
        public string Prefix { get; set; } = default!;

        [JsonPropertyName("Version")]
        public string Version { get; set; } = default!;
    }
}

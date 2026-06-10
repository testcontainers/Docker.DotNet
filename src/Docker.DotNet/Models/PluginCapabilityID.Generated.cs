#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginCapabilityID // (plugin.CapabilityID)
    {
        [JsonPropertyName("Capability")]
        public string Capability { get; set; } = string.Empty;

        [JsonPropertyName("Prefix")]
        public string Prefix { get; set; } = string.Empty;

        [JsonPropertyName("Version")]
        public string Version { get; set; } = string.Empty;
    }
}

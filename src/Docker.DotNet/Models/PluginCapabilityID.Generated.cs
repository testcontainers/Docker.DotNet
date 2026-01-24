namespace Docker.DotNet.Models
{
    public class PluginCapabilityID // (plugin.CapabilityID)
    {
        [JsonPropertyName("Capability")]
        public string Capability { get; set; }

        [JsonPropertyName("Prefix")]
        public string Prefix { get; set; }

        [JsonPropertyName("Version")]
        public string Version { get; set; }
    }
}

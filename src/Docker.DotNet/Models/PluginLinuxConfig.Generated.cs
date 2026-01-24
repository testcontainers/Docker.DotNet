namespace Docker.DotNet.Models
{
    public class PluginLinuxConfig // (plugin.LinuxConfig)
    {
        [JsonPropertyName("AllowAllDevices")]
        public bool AllowAllDevices { get; set; }

        [JsonPropertyName("Capabilities")]
        public IList<string> Capabilities { get; set; }

        [JsonPropertyName("Devices")]
        public IList<PluginDevice> Devices { get; set; }
    }
}

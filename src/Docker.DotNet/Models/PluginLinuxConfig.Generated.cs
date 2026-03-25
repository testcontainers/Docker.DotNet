#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginLinuxConfig // (plugin.LinuxConfig)
    {
        [JsonPropertyName("AllowAllDevices")]
        public bool AllowAllDevices { get; set; } = default!;

        [JsonPropertyName("Capabilities")]
        public IList<string> Capabilities { get; set; } = default!;

        [JsonPropertyName("Devices")]
        public IList<PluginDevice> Devices { get; set; } = default!;
    }
}

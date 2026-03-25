#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginSettings // (plugin.Settings)
    {
        [JsonPropertyName("Args")]
        public IList<string> Args { get; set; } = default!;

        [JsonPropertyName("Devices")]
        public IList<PluginDevice> Devices { get; set; } = default!;

        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; } = default!;

        [JsonPropertyName("Mounts")]
        public IList<PluginMount> Mounts { get; set; } = default!;
    }
}

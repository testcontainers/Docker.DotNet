#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Settings user-configurable settings for the plugin.
    /// 
    /// swagger:model Settings
    /// </summary>
    public class PluginSettings // (plugin.Settings)
    {
        /// <summary>
        /// args
        /// Required: true
        /// </summary>
        [JsonPropertyName("Args")]
        public IList<string> Args { get; set; } = default!;

        /// <summary>
        /// devices
        /// Required: true
        /// </summary>
        [JsonPropertyName("Devices")]
        public IList<PluginDevice> Devices { get; set; } = default!;

        /// <summary>
        /// env
        /// Example: [&quot;DEBUG=0&quot;]
        /// Required: true
        /// </summary>
        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; } = default!;

        /// <summary>
        /// mounts
        /// Required: true
        /// </summary>
        [JsonPropertyName("Mounts")]
        public IList<PluginMount> Mounts { get; set; } = default!;
    }
}

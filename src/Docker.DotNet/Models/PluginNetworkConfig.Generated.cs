#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkConfig network config
    /// 
    /// swagger:model NetworkConfig
    /// </summary>
    public class PluginNetworkConfig // (plugin.NetworkConfig)
    {
        /// <summary>
        /// type
        /// Example: host
        /// Required: true
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;
    }
}

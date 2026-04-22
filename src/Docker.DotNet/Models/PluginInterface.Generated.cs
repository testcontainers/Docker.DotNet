#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Interface The interface between Docker and the plugin
    /// 
    /// swagger:model Interface
    /// </summary>
    public class PluginInterface // (plugin.Interface)
    {
        /// <summary>
        /// Protocol to use for clients connecting to the plugin.
        /// Example: some.protocol/v1.0
        /// Enum: [&quot;&quot;,&quot;moby.plugins.http/v1&quot;]
        /// </summary>
        [JsonPropertyName("ProtocolScheme")]
        public string? ProtocolScheme { get; set; }

        /// <summary>
        /// socket
        /// Example: plugins.sock
        /// Required: true
        /// </summary>
        [JsonPropertyName("Socket")]
        public string Socket { get; set; } = default!;

        /// <summary>
        /// types
        /// Example: [&quot;docker.volumedriver/1.0&quot;]
        /// Required: true
        /// </summary>
        [JsonPropertyName("Types")]
        public IList<string> Types { get; set; } = default!;
    }
}

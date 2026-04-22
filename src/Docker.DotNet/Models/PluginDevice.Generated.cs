#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Device device
    /// 
    /// swagger:model Device
    /// </summary>
    public class PluginDevice // (plugin.Device)
    {
        /// <summary>
        /// description
        /// Required: true
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        /// <summary>
        /// name
        /// Required: true
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// path
        /// Example: /dev/fuse
        /// Required: true
        /// </summary>
        [JsonPropertyName("Path")]
        public string? Path { get; set; }

        /// <summary>
        /// settable
        /// Required: true
        /// </summary>
        [JsonPropertyName("Settable")]
        public IList<string> Settable { get; set; } = default!;
    }
}

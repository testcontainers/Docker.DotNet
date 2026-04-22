#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Mount mount
    /// 
    /// swagger:model Mount
    /// </summary>
    public class PluginMount // (plugin.Mount)
    {
        /// <summary>
        /// description
        /// Example: This is a mount that&apos;s used by the plugin.
        /// Required: true
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        /// <summary>
        /// destination
        /// Example: /mnt/state
        /// Required: true
        /// </summary>
        [JsonPropertyName("Destination")]
        public string Destination { get; set; } = default!;

        /// <summary>
        /// name
        /// Example: some-mount
        /// Required: true
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// options
        /// Example: [&quot;rbind&quot;,&quot;rw&quot;]
        /// Required: true
        /// </summary>
        [JsonPropertyName("Options")]
        public IList<string> Options { get; set; } = default!;

        /// <summary>
        /// settable
        /// Required: true
        /// </summary>
        [JsonPropertyName("Settable")]
        public IList<string> Settable { get; set; } = default!;

        /// <summary>
        /// source
        /// Example: /var/lib/docker/plugins/
        /// Required: true
        /// </summary>
        [JsonPropertyName("Source")]
        public string? Source { get; set; }

        /// <summary>
        /// type
        /// Example: bind
        /// Required: true
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;
    }
}

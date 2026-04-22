#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Args args
    /// 
    /// swagger:model Args
    /// </summary>
    public class PluginArgs // (plugin.Args)
    {
        /// <summary>
        /// description
        /// Example: command line arguments
        /// Required: true
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        /// <summary>
        /// name
        /// Example: args
        /// Required: true
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// settable
        /// Required: true
        /// </summary>
        [JsonPropertyName("Settable")]
        public IList<string> Settable { get; set; } = default!;

        /// <summary>
        /// value
        /// Required: true
        /// </summary>
        [JsonPropertyName("Value")]
        public IList<string> Value { get; set; } = default!;
    }
}

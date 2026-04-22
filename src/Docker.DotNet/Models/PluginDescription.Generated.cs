#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PluginDescription represents the description of an engine plugin.
    /// </summary>
    public class PluginDescription // (swarm.PluginDescription)
    {
        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}

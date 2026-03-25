#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginDescription // (swarm.PluginDescription)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;
    }
}

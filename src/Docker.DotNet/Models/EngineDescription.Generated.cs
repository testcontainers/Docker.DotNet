#nullable enable
namespace Docker.DotNet.Models
{
    public class EngineDescription // (swarm.EngineDescription)
    {
        [JsonPropertyName("EngineVersion")]
        public string EngineVersion { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Plugins")]
        public IList<PluginDescription> Plugins { get; set; } = default!;
    }
}

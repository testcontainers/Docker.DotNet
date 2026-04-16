#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// EngineDescription represents the description of an engine.
    /// </summary>
    public class EngineDescription // (swarm.EngineDescription)
    {
        [JsonPropertyName("EngineVersion")]
        public string? EngineVersion { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string>? Labels { get; set; }

        [JsonPropertyName("Plugins")]
        public IList<PluginDescription>? Plugins { get; set; }
    }
}

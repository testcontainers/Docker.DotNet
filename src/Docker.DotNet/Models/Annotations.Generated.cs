#nullable enable
namespace Docker.DotNet.Models
{
    public class Annotations // (swarm.Annotations)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;
    }
}

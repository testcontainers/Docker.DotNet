#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Annotations represents how to describe an object.
    /// </summary>
    public class Annotations // (swarm.Annotations)
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;
    }
}

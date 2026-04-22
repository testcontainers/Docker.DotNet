#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NodeSpec represents the spec of a node.
    /// </summary>
    public class NodeUpdateParameters // (swarm.NodeSpec)
    {
        public NodeUpdateParameters()
        {
        }

        public NodeUpdateParameters(Annotations Annotations)
        {
            if (Annotations != null)
            {
                this.Name = Annotations.Name;
                this.Labels = Annotations.Labels;
            }
        }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Role")]
        public string? Role { get; set; }

        [JsonPropertyName("Availability")]
        public string? Availability { get; set; }
    }
}

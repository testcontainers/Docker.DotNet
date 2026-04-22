#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Topology defines the CSI topology of this node. This type is a duplicate of
    /// [github.com/moby/moby/api/types/volume.Topology]. Because the type definition
    /// is so simple and to avoid complicated structure or circular imports, we just
    /// duplicate it here. See that type for full documentation
    /// </summary>
    public class Topology // (swarm.Topology)
    {
        [JsonPropertyName("Segments")]
        public IDictionary<string, string>? Segments { get; set; }
    }
}

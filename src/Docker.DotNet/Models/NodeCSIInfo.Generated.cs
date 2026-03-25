#nullable enable
namespace Docker.DotNet.Models
{
    public class NodeCSIInfo // (swarm.NodeCSIInfo)
    {
        [JsonPropertyName("PluginName")]
        public string PluginName { get; set; } = default!;

        [JsonPropertyName("NodeID")]
        public string NodeID { get; set; } = default!;

        [JsonPropertyName("MaxVolumesPerNode")]
        public long MaxVolumesPerNode { get; set; } = default!;

        [JsonPropertyName("AccessibleTopology")]
        public Topology? AccessibleTopology { get; set; }
    }
}

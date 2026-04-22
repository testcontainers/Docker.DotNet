#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NodeCSIInfo represents information about a CSI plugin available on the node
    /// </summary>
    public class NodeCSIInfo // (swarm.NodeCSIInfo)
    {
        /// <summary>
        /// PluginName is the name of the CSI plugin.
        /// </summary>
        [JsonPropertyName("PluginName")]
        public string? PluginName { get; set; }

        /// <summary>
        /// NodeID is the ID of the node as reported by the CSI plugin. This is
        /// different from the swarm node ID.
        /// </summary>
        [JsonPropertyName("NodeID")]
        public string? NodeID { get; set; }

        /// <summary>
        /// MaxVolumesPerNode is the maximum number of volumes that may be published
        /// to this node
        /// </summary>
        [JsonPropertyName("MaxVolumesPerNode")]
        public long? MaxVolumesPerNode { get; set; }

        /// <summary>
        /// AccessibleTopology indicates the location of this node in the CSI
        /// plugin&apos;s topology
        /// </summary>
        [JsonPropertyName("AccessibleTopology")]
        public Topology? AccessibleTopology { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PeerInfo represents one peer of an overlay network.
    /// 
    /// swagger:model PeerInfo
    /// </summary>
    public class PeerInfo // (network.PeerInfo)
    {
        /// <summary>
        /// ID of the peer-node in the Swarm cluster.
        /// Example: 6869d7c1732b
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// IP-address of the peer-node in the Swarm cluster.
        /// Example: 10.133.77.91
        /// </summary>
        [JsonPropertyName("IP")]
        public string IP { get; set; } = default!;
    }
}

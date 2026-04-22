#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Peer represents a peer.
    /// </summary>
    public class Peer // (swarm.Peer)
    {
        [JsonPropertyName("NodeID")]
        public string NodeID { get; set; } = default!;

        [JsonPropertyName("Addr")]
        public string Addr { get; set; } = default!;
    }
}

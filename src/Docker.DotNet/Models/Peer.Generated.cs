#nullable enable
namespace Docker.DotNet.Models
{
    public class Peer // (swarm.Peer)
    {
        [JsonPropertyName("NodeID")]
        public string NodeID { get; set; } = default!;

        [JsonPropertyName("Addr")]
        public string Addr { get; set; } = default!;
    }
}

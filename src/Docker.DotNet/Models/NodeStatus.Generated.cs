#nullable enable
namespace Docker.DotNet.Models
{
    public class NodeStatus // (swarm.NodeStatus)
    {
        [JsonPropertyName("State")]
        public string State { get; set; } = default!;

        [JsonPropertyName("Message")]
        public string Message { get; set; } = default!;

        [JsonPropertyName("Addr")]
        public string Addr { get; set; } = default!;
    }
}

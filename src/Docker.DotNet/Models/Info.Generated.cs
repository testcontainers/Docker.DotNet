#nullable enable
namespace Docker.DotNet.Models
{
    public class Info // (swarm.Info)
    {
        [JsonPropertyName("NodeID")]
        public string NodeID { get; set; } = default!;

        [JsonPropertyName("NodeAddr")]
        public string NodeAddr { get; set; } = default!;

        [JsonPropertyName("LocalNodeState")]
        public string LocalNodeState { get; set; } = default!;

        [JsonPropertyName("ControlAvailable")]
        public bool ControlAvailable { get; set; } = default!;

        [JsonPropertyName("Error")]
        public string Error { get; set; } = default!;

        [JsonPropertyName("RemoteManagers")]
        public IList<Peer> RemoteManagers { get; set; } = default!;

        [JsonPropertyName("Nodes")]
        public long Nodes { get; set; } = default!;

        [JsonPropertyName("Managers")]
        public long Managers { get; set; } = default!;

        [JsonPropertyName("Cluster")]
        public ClusterInfo? Cluster { get; set; }

        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}

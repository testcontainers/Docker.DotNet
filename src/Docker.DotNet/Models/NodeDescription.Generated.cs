#nullable enable
namespace Docker.DotNet.Models
{
    public class NodeDescription // (swarm.NodeDescription)
    {
        [JsonPropertyName("Hostname")]
        public string Hostname { get; set; } = default!;

        [JsonPropertyName("Platform")]
        public SwarmPlatform Platform { get; set; } = default!;

        [JsonPropertyName("Resources")]
        public SwarmResources Resources { get; set; } = default!;

        [JsonPropertyName("Engine")]
        public EngineDescription Engine { get; set; } = default!;

        [JsonPropertyName("TLSInfo")]
        public TLSInfo TLSInfo { get; set; } = default!;

        [JsonPropertyName("CSIInfo")]
        public IList<NodeCSIInfo> CSIInfo { get; set; } = default!;
    }
}

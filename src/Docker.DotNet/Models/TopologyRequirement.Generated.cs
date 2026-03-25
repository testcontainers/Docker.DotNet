#nullable enable
namespace Docker.DotNet.Models
{
    public class TopologyRequirement // (volume.TopologyRequirement)
    {
        [JsonPropertyName("Requisite")]
        public IList<VolumeTopology> Requisite { get; set; } = default!;

        [JsonPropertyName("Preferred")]
        public IList<VolumeTopology> Preferred { get; set; } = default!;
    }
}

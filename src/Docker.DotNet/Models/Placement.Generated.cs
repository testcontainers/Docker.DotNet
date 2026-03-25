#nullable enable
namespace Docker.DotNet.Models
{
    public class Placement // (swarm.Placement)
    {
        [JsonPropertyName("Constraints")]
        public IList<string> Constraints { get; set; } = default!;

        [JsonPropertyName("Preferences")]
        public IList<PlacementPreference> Preferences { get; set; } = default!;

        [JsonPropertyName("MaxReplicas")]
        public ulong MaxReplicas { get; set; } = default!;

        [JsonPropertyName("Platforms")]
        public IList<SwarmPlatform> Platforms { get; set; } = default!;
    }
}

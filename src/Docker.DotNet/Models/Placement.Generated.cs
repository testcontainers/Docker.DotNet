#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Placement represents orchestration parameters.
    /// </summary>
    public class Placement // (swarm.Placement)
    {
        [JsonPropertyName("Constraints")]
        public IList<string>? Constraints { get; set; }

        [JsonPropertyName("Preferences")]
        public IList<PlacementPreference>? Preferences { get; set; }

        [JsonPropertyName("MaxReplicas")]
        public ulong? MaxReplicas { get; set; }

        /// <summary>
        /// Platforms stores all the platforms that the image can run on.
        /// This field is used in the platform filter for scheduling. If empty,
        /// then the platform filter is off, meaning there are no scheduling restrictions.
        /// </summary>
        [JsonPropertyName("Platforms")]
        public IList<SwarmPlatform>? Platforms { get; set; }
    }
}

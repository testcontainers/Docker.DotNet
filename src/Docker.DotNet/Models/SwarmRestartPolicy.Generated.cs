#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmRestartPolicy // (swarm.RestartPolicy)
    {
        [JsonPropertyName("Condition")]
        public string? Condition { get; set; }

        [JsonPropertyName("Delay")]
        public TimeSpan? Delay { get; set; }

        [JsonPropertyName("MaxAttempts")]
        public ulong? MaxAttempts { get; set; }

        [JsonPropertyName("Window")]
        public TimeSpan? Window { get; set; }
    }
}

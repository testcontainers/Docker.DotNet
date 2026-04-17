#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmRestartPolicy // (swarm.RestartPolicy)
    {
        [JsonPropertyName("Condition")]
        public string? Condition { get; set; }

        [JsonPropertyName("Delay")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan? Delay { get; set; }

        [JsonPropertyName("MaxAttempts")]
        public ulong? MaxAttempts { get; set; }

        [JsonPropertyName("Window")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan? Window { get; set; }
    }
}

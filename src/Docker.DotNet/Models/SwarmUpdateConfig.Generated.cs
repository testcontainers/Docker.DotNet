#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmUpdateConfig // (swarm.UpdateConfig)
    {
        [JsonPropertyName("Parallelism")]
        public ulong Parallelism { get; set; } = default!;

        [JsonPropertyName("Delay")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan? Delay { get; set; }

        [JsonPropertyName("FailureAction")]
        public string? FailureAction { get; set; }

        [JsonPropertyName("Monitor")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan? Monitor { get; set; }

        [JsonPropertyName("MaxFailureRatio")]
        public float MaxFailureRatio { get; set; } = default!;

        [JsonPropertyName("Order")]
        public string Order { get; set; } = default!;
    }
}

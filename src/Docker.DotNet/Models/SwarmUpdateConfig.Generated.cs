#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmUpdateConfig // (swarm.UpdateConfig)
    {
        [JsonPropertyName("Parallelism")]
        public ulong Parallelism { get; set; } = default!;

        [JsonPropertyName("Delay")]
        public long Delay { get; set; } = default!;

        [JsonPropertyName("FailureAction")]
        public string FailureAction { get; set; } = default!;

        [JsonPropertyName("Monitor")]
        public long Monitor { get; set; } = default!;

        [JsonPropertyName("MaxFailureRatio")]
        public float MaxFailureRatio { get; set; } = default!;

        [JsonPropertyName("Order")]
        public string Order { get; set; } = default!;
    }
}

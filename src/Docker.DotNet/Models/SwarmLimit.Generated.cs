#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmLimit // (swarm.Limit)
    {
        [JsonPropertyName("NanoCPUs")]
        public long NanoCPUs { get; set; } = default!;

        [JsonPropertyName("MemoryBytes")]
        public long MemoryBytes { get; set; } = default!;

        [JsonPropertyName("Pids")]
        public long Pids { get; set; } = default!;
    }
}

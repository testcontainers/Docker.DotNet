#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Limit describes limits on resources which can be requested by a task.
    /// </summary>
    public class SwarmLimit // (swarm.Limit)
    {
        [JsonPropertyName("NanoCPUs")]
        public long? NanoCPUs { get; set; }

        [JsonPropertyName("MemoryBytes")]
        public long? MemoryBytes { get; set; }

        [JsonPropertyName("Pids")]
        public long? Pids { get; set; }
    }
}

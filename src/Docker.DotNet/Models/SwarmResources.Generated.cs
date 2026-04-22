#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Resources represents resources (CPU/Memory) which can be advertised by a
    /// node and requested to be reserved for a task.
    /// </summary>
    public class SwarmResources // (swarm.Resources)
    {
        [JsonPropertyName("NanoCPUs")]
        public long? NanoCPUs { get; set; }

        [JsonPropertyName("MemoryBytes")]
        public long? MemoryBytes { get; set; }

        [JsonPropertyName("GenericResources")]
        public IList<GenericResource>? GenericResources { get; set; }
    }
}

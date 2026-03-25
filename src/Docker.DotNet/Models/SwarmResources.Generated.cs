#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmResources // (swarm.Resources)
    {
        [JsonPropertyName("NanoCPUs")]
        public long NanoCPUs { get; set; } = default!;

        [JsonPropertyName("MemoryBytes")]
        public long MemoryBytes { get; set; } = default!;

        [JsonPropertyName("GenericResources")]
        public IList<GenericResource> GenericResources { get; set; } = default!;
    }
}

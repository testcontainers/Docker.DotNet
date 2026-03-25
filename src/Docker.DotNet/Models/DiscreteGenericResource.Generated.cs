#nullable enable
namespace Docker.DotNet.Models
{
    public class DiscreteGenericResource // (swarm.DiscreteGenericResource)
    {
        [JsonPropertyName("Kind")]
        public string Kind { get; set; } = default!;

        [JsonPropertyName("Value")]
        public long Value { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class JoinTokens // (swarm.JoinTokens)
    {
        [JsonPropertyName("Worker")]
        public string Worker { get; set; } = default!;

        [JsonPropertyName("Manager")]
        public string Manager { get; set; } = default!;
    }
}

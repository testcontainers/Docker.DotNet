#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// JoinTokens contains the tokens workers and managers need to join the swarm.
    /// </summary>
    public class JoinTokens // (swarm.JoinTokens)
    {
        /// <summary>
        /// Worker is the join token workers may use to join the swarm.
        /// </summary>
        [JsonPropertyName("Worker")]
        public string Worker { get; set; } = default!;

        /// <summary>
        /// Manager is the join token managers may use to join the swarm.
        /// </summary>
        [JsonPropertyName("Manager")]
        public string Manager { get; set; } = default!;
    }
}

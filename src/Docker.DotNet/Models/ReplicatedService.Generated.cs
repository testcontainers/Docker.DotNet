#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ReplicatedService is a kind of ServiceMode.
    /// </summary>
    public class ReplicatedService // (swarm.ReplicatedService)
    {
        [JsonPropertyName("Replicas")]
        public ulong? Replicas { get; set; }
    }
}

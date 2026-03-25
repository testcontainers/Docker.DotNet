#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworkAttachment // (swarm.NetworkAttachment)
    {
        [JsonPropertyName("Network")]
        public SwarmNetwork Network { get; set; } = default!;

        [JsonPropertyName("Addresses")]
        public IList<string> Addresses { get; set; } = default!;
    }
}

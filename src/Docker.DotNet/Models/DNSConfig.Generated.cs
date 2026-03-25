#nullable enable
namespace Docker.DotNet.Models
{
    public class DNSConfig // (swarm.DNSConfig)
    {
        [JsonPropertyName("Nameservers")]
        public IList<string> Nameservers { get; set; } = default!;

        [JsonPropertyName("Search")]
        public IList<string> Search { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IList<string> Options { get; set; } = default!;
    }
}

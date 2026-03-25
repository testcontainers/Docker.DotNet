#nullable enable
namespace Docker.DotNet.Models
{
    public class IPAMOptions // (swarm.IPAMOptions)
    {
        [JsonPropertyName("Driver")]
        public SwarmDriver Driver { get; set; } = default!;

        [JsonPropertyName("Configs")]
        public IList<SwarmIPAMConfig> Configs { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAMConfig represents ipam configuration.
    /// </summary>
    public class SwarmIPAMConfig // (swarm.IPAMConfig)
    {
        [JsonPropertyName("Subnet")]
        public string Subnet { get; set; } = default!;

        [JsonPropertyName("Range")]
        public string Range { get; set; } = default!;

        [JsonPropertyName("Gateway")]
        public string Gateway { get; set; } = default!;
    }
}

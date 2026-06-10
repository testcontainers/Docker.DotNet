#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAMConfig represents ipam configuration.
    /// </summary>
    public class SwarmIPAMConfig // (swarm.IPAMConfig)
    {
        [JsonPropertyName("Subnet")]
        public string Subnet { get; set; } = string.Empty;

        [JsonPropertyName("Range")]
        public string Range { get; set; } = string.Empty;

        [JsonPropertyName("Gateway")]
        public string Gateway { get; set; } = string.Empty;
    }
}

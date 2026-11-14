#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAMConfig represents IPAM configurations
    /// </summary>
    public class IPAMConfig // (network.IPAMConfig)
    {
        [JsonPropertyName("Subnet")]
        public string? Subnet { get; set; }

        [JsonPropertyName("IPRange")]
        public string? IPRange { get; set; }

        [JsonPropertyName("Gateway")]
        public string? Gateway { get; set; }

        [JsonPropertyName("AuxiliaryAddresses")]
        public IDictionary<string, string>? AuxAddress { get; set; }
    }
}

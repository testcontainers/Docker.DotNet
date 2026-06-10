#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAMConfig represents IPAM configurations
    /// </summary>
    public class IPAMConfig // (network.IPAMConfig)
    {
        [JsonPropertyName("Subnet")]
        public string Subnet { get; set; } = string.Empty;

        [JsonPropertyName("IPRange")]
        public string IPRange { get; set; } = string.Empty;

        [JsonPropertyName("Gateway")]
        public string Gateway { get; set; } = string.Empty;

        [JsonPropertyName("AuxiliaryAddresses")]
        public IDictionary<string, string>? AuxAddress { get; set; }
    }
}

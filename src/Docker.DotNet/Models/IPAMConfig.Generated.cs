#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAMConfig represents IPAM configurations
    /// </summary>
    public class IPAMConfig // (network.IPAMConfig)
    {
        [JsonPropertyName("Subnet")]
        public string Subnet { get; set; } = default!;

        [JsonPropertyName("IPRange")]
        public string IPRange { get; set; } = default!;

        [JsonPropertyName("Gateway")]
        public string Gateway { get; set; } = default!;

        [JsonPropertyName("AuxiliaryAddresses")]
        public IDictionary<string, string>? AuxAddress { get; set; }
    }
}

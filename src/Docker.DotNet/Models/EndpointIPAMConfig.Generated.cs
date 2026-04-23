#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// EndpointIPAMConfig represents IPAM configurations for the endpoint
    /// </summary>
    public class EndpointIPAMConfig // (network.EndpointIPAMConfig)
    {
        [JsonPropertyName("IPv4Address")]
        public string IPv4Address { get; set; } = default!;

        [JsonPropertyName("IPv6Address")]
        public string IPv6Address { get; set; } = default!;

        [JsonPropertyName("LinkLocalIPs")]
        public IList<string>? LinkLocalIPs { get; set; }
    }
}

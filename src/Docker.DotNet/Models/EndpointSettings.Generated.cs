#nullable enable
namespace Docker.DotNet.Models
{
    public class EndpointSettings // (network.EndpointSettings)
    {
        [JsonPropertyName("IPAMConfig")]
        public EndpointIPAMConfig? IPAMConfig { get; set; }

        [JsonPropertyName("Links")]
        public IList<string> Links { get; set; } = default!;

        [JsonPropertyName("Aliases")]
        public IList<string> Aliases { get; set; } = default!;

        [JsonPropertyName("DriverOpts")]
        public IDictionary<string, string> DriverOpts { get; set; } = default!;

        [JsonPropertyName("GwPriority")]
        public long GwPriority { get; set; } = default!;

        [JsonPropertyName("NetworkID")]
        public string NetworkID { get; set; } = default!;

        [JsonPropertyName("EndpointID")]
        public string EndpointID { get; set; } = default!;

        [JsonPropertyName("Gateway")]
        public string Gateway { get; set; } = default!;

        [JsonPropertyName("IPAddress")]
        public string IPAddress { get; set; } = default!;

        [JsonPropertyName("MacAddress")]
        public string MacAddress { get; set; } = default!;

        [JsonPropertyName("IPPrefixLen")]
        public long IPPrefixLen { get; set; } = default!;

        [JsonPropertyName("IPv6Gateway")]
        public string IPv6Gateway { get; set; } = default!;

        [JsonPropertyName("GlobalIPv6Address")]
        public string GlobalIPv6Address { get; set; } = default!;

        [JsonPropertyName("GlobalIPv6PrefixLen")]
        public long GlobalIPv6PrefixLen { get; set; } = default!;

        [JsonPropertyName("DNSNames")]
        public IList<string> DNSNames { get; set; } = default!;
    }
}

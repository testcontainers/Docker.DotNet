#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// EndpointSettings stores the network endpoint details
    /// </summary>
    public class EndpointSettings // (network.EndpointSettings)
    {
        /// <summary>
        /// Configuration data
        /// </summary>
        [JsonPropertyName("IPAMConfig")]
        public EndpointIPAMConfig? IPAMConfig { get; set; }

        [JsonPropertyName("Links")]
        public IList<string> Links { get; set; } = default!;

        /// <summary>
        /// Aliases holds the list of extra, user-specified DNS names for this endpoint.
        /// </summary>
        [JsonPropertyName("Aliases")]
        public IList<string> Aliases { get; set; } = default!;

        [JsonPropertyName("DriverOpts")]
        public IDictionary<string, string> DriverOpts { get; set; } = default!;

        /// <summary>
        /// GwPriority determines which endpoint will provide the default gateway
        /// for the container. The endpoint with the highest priority will be used.
        /// If multiple endpoints have the same priority, they are lexicographically
        /// sorted based on their network name, and the one that sorts first is picked.
        /// </summary>
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

        /// <summary>
        /// MacAddress may be used to specify a MAC address when the container is created.
        /// Once the container is running, it becomes operational data (it may contain a
        /// generated address).
        /// </summary>
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

        /// <summary>
        /// DNSNames holds all the (non fully qualified) DNS names associated to this
        /// endpoint. The first entry is used to generate PTR records.
        /// </summary>
        [JsonPropertyName("DNSNames")]
        public IList<string> DNSNames { get; set; } = default!;
    }
}

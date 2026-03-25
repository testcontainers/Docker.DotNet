#nullable enable
namespace Docker.DotNet.Models
{
    public class EndpointResource // (network.EndpointResource)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("EndpointID")]
        public string EndpointID { get; set; } = default!;

        [JsonPropertyName("MacAddress")]
        public string MacAddress { get; set; } = default!;

        [JsonPropertyName("IPv4Address")]
        public string IPv4Address { get; set; } = default!;

        [JsonPropertyName("IPv6Address")]
        public string IPv6Address { get; set; } = default!;
    }
}

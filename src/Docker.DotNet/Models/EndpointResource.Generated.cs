#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// EndpointResource contains network resources allocated and used for a container in a network.
    /// 
    /// swagger:model EndpointResource
    /// </summary>
    public class EndpointResource // (network.EndpointResource)
    {
        /// <summary>
        /// name
        /// Example: container_1
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// endpoint ID
        /// Example: 628cadb8bcb92de107b2a1e516cbffe463e321f548feb37697cce00ad694f21a
        /// </summary>
        [JsonPropertyName("EndpointID")]
        public string EndpointID { get; set; } = default!;

        /// <summary>
        /// mac address
        /// Example: 02:42:ac:13:00:02
        /// </summary>
        [JsonPropertyName("MacAddress")]
        public string MacAddress { get; set; } = default!;

        /// <summary>
        /// IPv4 address
        /// Example: 172.19.0.2/16
        /// </summary>
        [JsonPropertyName("IPv4Address")]
        public string IPv4Address { get; set; } = default!;

        /// <summary>
        /// IPv6 address
        /// </summary>
        [JsonPropertyName("IPv6Address")]
        public string IPv6Address { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// EndpointVirtualIP represents the virtual ip of a port.
    /// </summary>
    public class EndpointVirtualIP // (swarm.EndpointVirtualIP)
    {
        [JsonPropertyName("NetworkID")]
        public string? NetworkID { get; set; }

        /// <summary>
        /// Addr is the virtual ip address.
        /// This field accepts CIDR notation, for example `10.0.0.1/24`, to maintain backwards
        /// compatibility, but only the IP address is used.
        /// </summary>
        [JsonPropertyName("Addr")]
        public string? Addr { get; set; }
    }
}

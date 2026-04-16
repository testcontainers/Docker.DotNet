#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkAttachment represents a network attachment.
    /// </summary>
    public class NetworkAttachment // (swarm.NetworkAttachment)
    {
        [JsonPropertyName("Network")]
        public SwarmNetwork? Network { get; set; }

        /// <summary>
        /// Addresses contains the IP addresses associated with the endpoint in the network.
        /// This field accepts CIDR notation, for example `10.0.0.1/24`, to maintain backwards
        /// compatibility, but only the IP address is used.
        /// </summary>
        [JsonPropertyName("Addresses")]
        public IList<string>? Addresses { get; set; }
    }
}

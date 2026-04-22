#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Endpoint represents an endpoint.
    /// </summary>
    public class Endpoint // (swarm.Endpoint)
    {
        [JsonPropertyName("Spec")]
        public EndpointSpec? Spec { get; set; }

        [JsonPropertyName("Ports")]
        public IList<PortConfig>? Ports { get; set; }

        [JsonPropertyName("VirtualIPs")]
        public IList<EndpointVirtualIP>? VirtualIPs { get; set; }
    }
}

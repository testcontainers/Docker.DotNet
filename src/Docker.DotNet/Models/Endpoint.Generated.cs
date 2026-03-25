#nullable enable
namespace Docker.DotNet.Models
{
    public class Endpoint // (swarm.Endpoint)
    {
        [JsonPropertyName("Spec")]
        public EndpointSpec Spec { get; set; } = default!;

        [JsonPropertyName("Ports")]
        public IList<PortConfig> Ports { get; set; } = default!;

        [JsonPropertyName("VirtualIPs")]
        public IList<EndpointVirtualIP> VirtualIPs { get; set; } = default!;
    }
}

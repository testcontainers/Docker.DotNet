#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// EndpointSpec represents the spec of an endpoint.
    /// </summary>
    public class EndpointSpec // (swarm.EndpointSpec)
    {
        [JsonPropertyName("Mode")]
        public string? Mode { get; set; }

        [JsonPropertyName("Ports")]
        public IList<PortConfig>? Ports { get; set; }
    }
}

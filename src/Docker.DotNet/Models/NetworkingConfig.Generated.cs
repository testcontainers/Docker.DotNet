#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkingConfig represents the container&apos;s networking configuration for each of its interfaces
    /// Carries the networking configs specified in the `docker run` and `docker network connect` commands
    /// </summary>
    public class NetworkingConfig // (network.NetworkingConfig)
    {
        [JsonPropertyName("EndpointsConfig")]
        public IDictionary<string, EndpointSettings> EndpointsConfig { get; set; } = default!;
    }
}

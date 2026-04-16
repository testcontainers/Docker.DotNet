#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkSettings exposes the network settings in the api
    /// </summary>
    public class NetworkSettings // (container.NetworkSettings)
    {
        [JsonPropertyName("SandboxID")]
        public string SandboxID { get; set; } = default!;

        [JsonPropertyName("SandboxKey")]
        public string SandboxKey { get; set; } = default!;

        /// <summary>
        /// Ports is a collection of [network.PortBinding] indexed by [network.Port]
        /// </summary>
        [JsonPropertyName("Ports")]
        public IDictionary<string, IList<PortBinding>> Ports { get; set; } = default!;

        [JsonPropertyName("Networks")]
        public IDictionary<string, EndpointSettings> Networks { get; set; } = default!;
    }
}

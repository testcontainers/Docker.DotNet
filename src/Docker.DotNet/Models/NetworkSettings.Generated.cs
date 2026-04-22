#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkSettings exposes the network settings in the api
    /// </summary>
    public class NetworkSettings // (container.NetworkSettings)
    {
        /// <summary>
        /// SandboxID uniquely represents a container&apos;s network stack
        /// </summary>
        [JsonPropertyName("SandboxID")]
        public string SandboxID { get; set; } = default!;

        /// <summary>
        /// SandboxKey identifies the sandbox
        /// </summary>
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

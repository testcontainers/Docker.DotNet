#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworkSettings // (container.NetworkSettings)
    {
        [JsonPropertyName("SandboxID")]
        public string SandboxID { get; set; } = default!;

        [JsonPropertyName("SandboxKey")]
        public string SandboxKey { get; set; } = default!;

        [JsonPropertyName("Ports")]
        public IDictionary<string, IList<PortBinding>> Ports { get; set; } = default!;

        [JsonPropertyName("Networks")]
        public IDictionary<string, EndpointSettings> Networks { get; set; } = default!;
    }
}

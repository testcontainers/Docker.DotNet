namespace Docker.DotNet.Models
{
    public class NetworkSettings // (container.NetworkSettings)
    {
        [JsonPropertyName("SandboxID")]
        public string SandboxID { get; set; }

        [JsonPropertyName("SandboxKey")]
        public string SandboxKey { get; set; }

        [JsonPropertyName("Ports")]
        public IDictionary<string, IList<PortBinding>> Ports { get; set; }

        [JsonPropertyName("Networks")]
        public IDictionary<string, EndpointSettings> Networks { get; set; }
    }
}

namespace Docker.DotNet.Models
{
    public class PortBinding // (network.PortBinding)
    {
        [JsonPropertyName("HostIp")]
        public string HostIP { get; set; }

        [JsonPropertyName("HostPort")]
        public string HostPort { get; set; }
    }
}

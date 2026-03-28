#nullable enable
namespace Docker.DotNet.Models
{
    public class PortBinding // (network.PortBinding)
    {
        [JsonPropertyName("HostIp")]
        public string HostIP { get; set; } = default!;

        [JsonPropertyName("HostPort")]
        public string HostPort { get; set; } = default!;
    }
}

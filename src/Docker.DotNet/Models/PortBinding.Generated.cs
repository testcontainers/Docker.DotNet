#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PortBinding represents a binding between a Host IP address and a Host Port.
    /// </summary>
    public class PortBinding // (network.PortBinding)
    {
        /// <summary>
        /// HostIP is the host IP Address
        /// </summary>
        [JsonPropertyName("HostIp")]
        public string HostIP { get; set; } = default!;

        /// <summary>
        /// HostPort is the host port number
        /// </summary>
        [JsonPropertyName("HostPort")]
        public string HostPort { get; set; } = default!;
    }
}

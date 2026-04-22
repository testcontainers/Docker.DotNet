#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PortStatus represents the port status of a task&apos;s host ports whose
    /// service has published host ports
    /// </summary>
    public class PortStatus // (swarm.PortStatus)
    {
        [JsonPropertyName("Ports")]
        public IList<PortConfig>? Ports { get; set; }
    }
}

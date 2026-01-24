namespace Docker.DotNet.Models
{
    public class PortStatus // (swarm.PortStatus)
    {
        [JsonPropertyName("Ports")]
        public IList<PortConfig> Ports { get; set; }
    }
}

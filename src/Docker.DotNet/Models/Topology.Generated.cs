namespace Docker.DotNet.Models
{
    public class Topology // (swarm.Topology)
    {
        [JsonPropertyName("Segments")]
        public IDictionary<string, string> Segments { get; set; }
    }
}

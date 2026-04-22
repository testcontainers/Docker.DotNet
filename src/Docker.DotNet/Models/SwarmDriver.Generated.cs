#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Driver represents a driver (network, logging, secrets backend).
    /// </summary>
    public class SwarmDriver // (swarm.Driver)
    {
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Options")]
        public IDictionary<string, string>? Options { get; set; }
    }
}

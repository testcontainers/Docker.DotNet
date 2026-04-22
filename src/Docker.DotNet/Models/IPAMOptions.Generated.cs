#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAMOptions represents ipam options.
    /// </summary>
    public class IPAMOptions // (swarm.IPAMOptions)
    {
        [JsonPropertyName("Driver")]
        public SwarmDriver? Driver { get; set; }

        [JsonPropertyName("Configs")]
        public IList<SwarmIPAMConfig>? Configs { get; set; }
    }
}

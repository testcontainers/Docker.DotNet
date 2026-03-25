#nullable enable
namespace Docker.DotNet.Models
{
    public class PeerInfo // (network.PeerInfo)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("IP")]
        public string IP { get; set; } = default!;
    }
}

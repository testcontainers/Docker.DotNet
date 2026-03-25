#nullable enable
namespace Docker.DotNet.Models
{
    public class ManagerStatus // (swarm.ManagerStatus)
    {
        [JsonPropertyName("Leader")]
        public bool Leader { get; set; } = default!;

        [JsonPropertyName("Reachability")]
        public string Reachability { get; set; } = default!;

        [JsonPropertyName("Addr")]
        public string Addr { get; set; } = default!;
    }
}

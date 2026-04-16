#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ManagerStatus represents the status of a manager.
    /// </summary>
    public class ManagerStatus // (swarm.ManagerStatus)
    {
        [JsonPropertyName("Leader")]
        public bool? Leader { get; set; }

        [JsonPropertyName("Reachability")]
        public string? Reachability { get; set; }

        [JsonPropertyName("Addr")]
        public string? Addr { get; set; }
    }
}

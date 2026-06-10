#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// JoinRequest is the request used to join a swarm.
    /// </summary>
    public class SwarmJoinParameters // (swarm.JoinRequest)
    {
        [JsonPropertyName("ListenAddr")]
        public string ListenAddr { get; set; } = string.Empty;

        [JsonPropertyName("AdvertiseAddr")]
        public string AdvertiseAddr { get; set; } = string.Empty;

        [JsonPropertyName("DataPathAddr")]
        public string DataPathAddr { get; set; } = string.Empty;

        [JsonPropertyName("RemoteAddrs")]
        public IList<string> RemoteAddrs { get; set; } = default!;

        /// <summary>
        /// accept by secret
        /// </summary>
        [JsonPropertyName("JoinToken")]
        public string JoinToken { get; set; } = string.Empty;

        [JsonPropertyName("Availability")]
        public string Availability { get; set; } = string.Empty;
    }
}

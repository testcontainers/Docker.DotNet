#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// JoinRequest is the request used to join a swarm.
    /// </summary>
    public class SwarmJoinParameters // (swarm.JoinRequest)
    {
        [JsonPropertyName("ListenAddr")]
        public string ListenAddr { get; set; } = default!;

        [JsonPropertyName("AdvertiseAddr")]
        public string AdvertiseAddr { get; set; } = default!;

        [JsonPropertyName("DataPathAddr")]
        public string DataPathAddr { get; set; } = default!;

        [JsonPropertyName("RemoteAddrs")]
        public IList<string> RemoteAddrs { get; set; } = default!;

        /// <summary>
        /// accept by secret
        /// </summary>
        [JsonPropertyName("JoinToken")]
        public string JoinToken { get; set; } = default!;

        [JsonPropertyName("Availability")]
        public string Availability { get; set; } = default!;
    }
}

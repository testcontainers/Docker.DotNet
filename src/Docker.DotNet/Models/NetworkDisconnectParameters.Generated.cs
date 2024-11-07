using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class NetworkDisconnectParameters // (network.DisconnectOptions)
    {
        [JsonPropertyName("Container")]
        public string Container { get; set; }

        [JsonPropertyName("Force")]
        public bool Force { get; set; }
    }
}

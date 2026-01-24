namespace Docker.DotNet.Models
{
    public class NetworkDisconnectParameters // (client.NetworkDisconnectOptions)
    {
        [JsonPropertyName("Container")]
        public string Container { get; set; }

        [JsonPropertyName("Force")]
        public bool Force { get; set; }
    }
}

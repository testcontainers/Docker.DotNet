#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkDisconnectOptions represents the data to be used to disconnect a container
    /// from the network.
    /// </summary>
    public class NetworkDisconnectParameters // (client.NetworkDisconnectOptions)
    {
        [JsonPropertyName("Container")]
        public string Container { get; set; } = default!;

        [JsonPropertyName("Force")]
        public bool Force { get; set; } = default!;
    }
}

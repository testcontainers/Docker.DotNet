#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkConnectOptions represents the data to be used to connect a container to the
    /// network.
    /// </summary>
    public class NetworkConnectParameters // (client.NetworkConnectOptions)
    {
        [JsonPropertyName("Container")]
        public string Container { get; set; } = default!;

        [JsonPropertyName("EndpointConfig")]
        public EndpointSettings? EndpointConfig { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworkConnectParameters // (client.NetworkConnectOptions)
    {
        [JsonPropertyName("Container")]
        public string Container { get; set; } = default!;

        [JsonPropertyName("EndpointConfig")]
        public EndpointSettings? EndpointConfig { get; set; }
    }
}

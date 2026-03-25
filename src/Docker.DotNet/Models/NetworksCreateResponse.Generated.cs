#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworksCreateResponse // (network.CreateResponse)
    {
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Warning")]
        public string Warning { get; set; } = default!;
    }
}

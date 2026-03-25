#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworkTask // (network.Task)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("EndpointID")]
        public string EndpointID { get; set; } = default!;

        [JsonPropertyName("EndpointIP")]
        public string EndpointIP { get; set; } = default!;

        [JsonPropertyName("Info")]
        public IDictionary<string, string> Info { get; set; } = default!;
    }
}

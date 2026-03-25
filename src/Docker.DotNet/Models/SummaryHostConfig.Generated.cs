#nullable enable
namespace Docker.DotNet.Models
{
    public class SummaryHostConfig // (container.Summary.HostConfig)
    {
        [JsonPropertyName("NetworkMode")]
        public string NetworkMode { get; set; } = default!;

        [JsonPropertyName("Annotations")]
        public IDictionary<string, string> Annotations { get; set; } = default!;
    }
}

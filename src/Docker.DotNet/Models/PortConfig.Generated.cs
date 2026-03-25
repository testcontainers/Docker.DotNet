#nullable enable
namespace Docker.DotNet.Models
{
    public class PortConfig // (swarm.PortConfig)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Protocol")]
        public string Protocol { get; set; } = default!;

        [JsonPropertyName("TargetPort")]
        public uint TargetPort { get; set; } = default!;

        [JsonPropertyName("PublishedPort")]
        public uint PublishedPort { get; set; } = default!;

        [JsonPropertyName("PublishMode")]
        public string PublishMode { get; set; } = default!;
    }
}

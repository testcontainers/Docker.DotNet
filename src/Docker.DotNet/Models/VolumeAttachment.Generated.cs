#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeAttachment // (swarm.VolumeAttachment)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Source")]
        public string Source { get; set; } = default!;

        [JsonPropertyName("Target")]
        public string Target { get; set; } = default!;
    }
}

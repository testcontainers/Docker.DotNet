#nullable enable
namespace Docker.DotNet.Models
{
    public class MountPoint // (container.MountPoint)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Source")]
        public string Source { get; set; } = default!;

        [JsonPropertyName("Destination")]
        public string Destination { get; set; } = default!;

        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("Mode")]
        public string Mode { get; set; } = default!;

        [JsonPropertyName("RW")]
        public bool RW { get; set; } = default!;

        [JsonPropertyName("Propagation")]
        public string Propagation { get; set; } = default!;
    }
}

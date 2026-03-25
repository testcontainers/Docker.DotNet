#nullable enable
namespace Docker.DotNet.Models
{
    public class ManifestSummary // (image.ManifestSummary)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Descriptor")]
        public Descriptor Descriptor { get; set; } = default!;

        [JsonPropertyName("Available")]
        public bool Available { get; set; } = default!;

        [JsonPropertyName("Size")]
        public ManifestSummarySize Size { get; set; } = default!;

        [JsonPropertyName("Kind")]
        public string Kind { get; set; } = default!;

        [JsonPropertyName("ImageData")]
        public ImageProperties? ImageData { get; set; }

        [JsonPropertyName("AttestationData")]
        public AttestationProperties? AttestationData { get; set; }
    }
}

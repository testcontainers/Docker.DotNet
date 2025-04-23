using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class ManifestSummary // (image.ManifestSummary)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("Descriptor")]
        public Descriptor Descriptor { get; set; }

        [JsonPropertyName("Available")]
        public bool Available { get; set; }

        [JsonPropertyName("Size")]
        public ManifestSummarySize Size { get; set; }

        [JsonPropertyName("Kind")]
        public string Kind { get; set; }

        [JsonPropertyName("ImageData")]
        public ImageProperties ImageData { get; set; }

        [JsonPropertyName("AttestationData")]
        public AttestationProperties AttestationData { get; set; }
    }
}

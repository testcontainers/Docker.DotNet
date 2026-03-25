#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageInspectResponse // (image.InspectResponse)
    {
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("RepoTags")]
        public IList<string> RepoTags { get; set; } = default!;

        [JsonPropertyName("RepoDigests")]
        public IList<string> RepoDigests { get; set; } = default!;

        [JsonPropertyName("Comment")]
        public string Comment { get; set; } = default!;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        [JsonPropertyName("Author")]
        public string Author { get; set; } = default!;

        [JsonPropertyName("Config")]
        public DockerOCIImageConfig? Config { get; set; }

        [JsonPropertyName("Architecture")]
        public string Architecture { get; set; } = default!;

        [JsonPropertyName("Variant")]
        public string Variant { get; set; } = default!;

        [JsonPropertyName("Os")]
        public string Os { get; set; } = default!;

        [JsonPropertyName("OsVersion")]
        public string OsVersion { get; set; } = default!;

        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;

        [JsonPropertyName("GraphDriver")]
        public DriverData? GraphDriver { get; set; }

        [JsonPropertyName("RootFS")]
        public RootFS RootFS { get; set; } = default!;

        [JsonPropertyName("Metadata")]
        public Metadata Metadata { get; set; } = default!;

        [JsonPropertyName("Descriptor")]
        public Descriptor? Descriptor { get; set; }

        [JsonPropertyName("Manifests")]
        public IList<ManifestSummary> Manifests { get; set; } = default!;
    }
}

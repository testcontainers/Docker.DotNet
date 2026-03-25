#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesListResponse // (image.Summary)
    {
        [JsonPropertyName("Containers")]
        public long Containers { get; set; } = default!;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("ParentId")]
        public string ParentID { get; set; } = default!;

        [JsonPropertyName("Descriptor")]
        public Descriptor? Descriptor { get; set; }

        [JsonPropertyName("Manifests")]
        public IList<ManifestSummary> Manifests { get; set; } = default!;

        [JsonPropertyName("RepoDigests")]
        public IList<string> RepoDigests { get; set; } = default!;

        [JsonPropertyName("RepoTags")]
        public IList<string> RepoTags { get; set; } = default!;

        [JsonPropertyName("SharedSize")]
        public long SharedSize { get; set; } = default!;

        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;
    }
}

namespace Docker.DotNet.Models
{
    public class ImageInspectResponse // (image.InspectResponse)
    {
        [JsonPropertyName("Id")]
        public string ID { get; set; }

        [JsonPropertyName("RepoTags")]
        public IList<string> RepoTags { get; set; }

        [JsonPropertyName("RepoDigests")]
        public IList<string> RepoDigests { get; set; }

        [JsonPropertyName("Parent")]
        public string Parent { get; set; }

        [JsonPropertyName("Comment")]
        public string Comment { get; set; }

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("Container")]
        public string Container { get; set; }

        [JsonPropertyName("ContainerConfig")]
        public ContainerConfig ContainerConfig { get; set; }

        [JsonPropertyName("DockerVersion")]
        public string DockerVersion { get; set; }

        [JsonPropertyName("Author")]
        public string Author { get; set; }

        [JsonPropertyName("Config")]
        public DockerOCIImageConfig Config { get; set; }

        [JsonPropertyName("Architecture")]
        public string Architecture { get; set; }

        [JsonPropertyName("Variant")]
        public string Variant { get; set; }

        [JsonPropertyName("Os")]
        public string Os { get; set; }

        [JsonPropertyName("OsVersion")]
        public string OsVersion { get; set; }

        [JsonPropertyName("Size")]
        public long Size { get; set; }

        [JsonPropertyName("VirtualSize")]
        public long VirtualSize { get; set; }

        [JsonPropertyName("GraphDriver")]
        public DriverData GraphDriver { get; set; }

        [JsonPropertyName("RootFS")]
        public RootFS RootFS { get; set; }

        [JsonPropertyName("Metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("Descriptor")]
        public Descriptor Descriptor { get; set; }

        [JsonPropertyName("Manifests")]
        public IList<ManifestSummary> Manifests { get; set; }
    }
}

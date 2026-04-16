#nullable enable
namespace Docker.DotNet.Models
{
    public class ManifestSummary // (image.ManifestSummary)
    {
        /// <summary>
        /// ID is the content-addressable ID of an image and is the same as the
        /// digest of the image manifest.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        /// <summary>
        /// Descriptor is the OCI descriptor of the image.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Descriptor")]
        public Descriptor Descriptor { get; set; } = default!;

        /// <summary>
        /// Indicates whether all the child content (image config, layers) is
        /// fully available locally
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Available")]
        public bool Available { get; set; } = default!;

        /// <summary>
        /// Size is the size information of the content related to this manifest.
        /// Note: These sizes only take the locally available content into account.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Size")]
        public ManifestSummarySize Size { get; set; } = default!;

        /// <summary>
        /// Kind is the kind of the image manifest.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Kind")]
        public string Kind { get; set; } = default!;

        /// <summary>
        /// Present only if Kind == ManifestKindImage.
        /// </summary>
        [JsonPropertyName("ImageData")]
        public ImageProperties? ImageData { get; set; }

        /// <summary>
        /// Present only if Kind == ManifestKindAttestation.
        /// </summary>
        [JsonPropertyName("AttestationData")]
        public AttestationProperties? AttestationData { get; set; }
    }
}

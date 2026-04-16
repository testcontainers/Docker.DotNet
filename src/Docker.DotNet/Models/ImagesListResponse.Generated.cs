#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesListResponse // (image.Summary)
    {
        /// <summary>
        /// Number of containers using this image. Includes both stopped and running
        /// containers.
        /// 
        /// This size is not calculated by default, and depends on which API endpoint
        /// is used. `-1` indicates that the value has not been set / calculated.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Containers")]
        public long Containers { get; set; } = default!;

        /// <summary>
        /// Date and time at which the image was created as a Unix timestamp
        /// (number of seconds since EPOCH).
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        /// <summary>
        /// ID is the content-addressable ID of an image.
        /// 
        /// This identifier is a content-addressable digest calculated from the
        /// image&apos;s configuration (which includes the digests of layers used by
        /// the image).
        /// 
        /// Note that this digest differs from the `RepoDigests` below, which
        /// holds digests of image manifests that reference the image.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        /// <summary>
        /// User-defined key/value metadata.
        /// Required: true
        /// </summary>
        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        /// <summary>
        /// ID of the parent image.
        /// 
        /// Depending on how the image was created, this field may be empty and
        /// is only set for images that were built/created locally. This field
        /// is empty if the image was pulled from an image registry.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("ParentId")]
        public string ParentID { get; set; } = default!;

        /// <summary>
        /// Descriptor is the OCI descriptor of the image target.
        /// It&apos;s only set if the daemon provides a multi-platform image store.
        /// 
        /// WARNING: This is experimental and may change at any time without any backward
        /// compatibility.
        /// </summary>
        [JsonPropertyName("Descriptor")]
        public Descriptor? Descriptor { get; set; }

        /// <summary>
        /// Manifests is a list of image manifests available in this image.  It
        /// provides a more detailed view of the platform-specific image manifests or
        /// other image-attached data like build attestations.
        /// 
        /// WARNING: This is experimental and may change at any time without any backward
        /// compatibility.
        /// </summary>
        [JsonPropertyName("Manifests")]
        public IList<ManifestSummary>? Manifests { get; set; }

        /// <summary>
        /// List of content-addressable digests of locally available image manifests
        /// that the image is referenced from. Multiple manifests can refer to the
        /// same image.
        /// 
        /// These digests are usually only available if the image was either pulled
        /// from a registry, or if the image was pushed to a registry, which is when
        /// the manifest is generated and its digest calculated.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("RepoDigests")]
        public IList<string> RepoDigests { get; set; } = default!;

        /// <summary>
        /// List of image names/tags in the local image cache that reference this
        /// image.
        /// 
        /// Multiple image tags can refer to the same image, and this list may be
        /// empty if no tags reference the image, in which case the image is
        /// &quot;untagged&quot;, in which case it can still be referenced by its ID.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("RepoTags")]
        public IList<string> RepoTags { get; set; } = default!;

        /// <summary>
        /// Total size of image layers that are shared between this image and other
        /// images.
        /// 
        /// This size is not calculated by default. `-1` indicates that the value
        /// has not been set / calculated.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("SharedSize")]
        public long SharedSize { get; set; } = default!;

        /// <summary>
        /// Total size of the image including all layers it is composed of.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;
    }
}

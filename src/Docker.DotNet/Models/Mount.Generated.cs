#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Mount represents a mount (volume).
    /// </summary>
    public class Mount // (mount.Mount)
    {
        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        /// <summary>
        /// Source specifies the name of the mount. Depending on mount type, this
        /// may be a volume name or a host path, or even ignored.
        /// Source is not supported for tmpfs (must be an empty value)
        /// </summary>
        [JsonPropertyName("Source")]
        public string? Source { get; set; }

        [JsonPropertyName("Target")]
        public string? Target { get; set; }

        [JsonPropertyName("ReadOnly")]
        public bool? ReadOnly { get; set; }

        [JsonPropertyName("Consistency")]
        public string? Consistency { get; set; }

        [JsonPropertyName("BindOptions")]
        public BindOptions? BindOptions { get; set; }

        [JsonPropertyName("VolumeOptions")]
        public VolumeOptions? VolumeOptions { get; set; }

        [JsonPropertyName("ImageOptions")]
        public ImageOptions? ImageOptions { get; set; }

        [JsonPropertyName("TmpfsOptions")]
        public TmpfsOptions? TmpfsOptions { get; set; }

        [JsonPropertyName("ClusterOptions")]
        public ClusterOptions? ClusterOptions { get; set; }
    }
}

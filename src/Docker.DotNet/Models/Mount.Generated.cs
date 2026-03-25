#nullable enable
namespace Docker.DotNet.Models
{
    public class Mount // (mount.Mount)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Source")]
        public string Source { get; set; } = default!;

        [JsonPropertyName("Target")]
        public string Target { get; set; } = default!;

        [JsonPropertyName("ReadOnly")]
        public bool ReadOnly { get; set; } = default!;

        [JsonPropertyName("Consistency")]
        public string Consistency { get; set; } = default!;

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

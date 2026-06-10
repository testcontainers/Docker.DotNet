#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeResponse // (main.VolumeResponse)
    {
        [JsonPropertyName("ClusterVolume")]
        public ClusterVolume? ClusterVolume { get; set; }

        [JsonPropertyName("CreatedAt")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = string.Empty;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Mountpoint")]
        public string Mountpoint { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public IDictionary<string, object>? Status { get; set; }

        [JsonPropertyName("UsageData")]
        public UsageData? UsageData { get; set; }
    }
}

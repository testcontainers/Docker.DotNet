#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeResponse // (main.VolumeResponse)
    {
        [JsonPropertyName("ClusterVolume")]
        public ClusterVolume? ClusterVolume { get; set; }

        [JsonPropertyName("CreatedAt")]
        public string CreatedAt { get; set; } = default!;

        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Mountpoint")]
        public string Mountpoint { get; set; } = default!;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = default!;

        [JsonPropertyName("Status")]
        public IDictionary<string, object> Status { get; set; } = default!;

        [JsonPropertyName("UsageData")]
        public UsageData? UsageData { get; set; }
    }
}

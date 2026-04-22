#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Volume volume
    /// 
    /// swagger:model Volume
    /// </summary>
    public class Volume // (volume.Volume)
    {
        /// <summary>
        /// cluster volume
        /// </summary>
        [JsonPropertyName("ClusterVolume")]
        public ClusterVolume? ClusterVolume { get; set; }

        /// <summary>
        /// Date/Time the volume was created.
        /// Example: 2016-06-07T20:31:11.853781916Z
        /// </summary>
        [JsonPropertyName("CreatedAt")]
        public string? CreatedAt { get; set; }

        /// <summary>
        /// Name of the volume driver used by the volume.
        /// Example: custom
        /// Required: true
        /// </summary>
        [JsonPropertyName("Driver")]
        public string Driver { get; set; } = default!;

        /// <summary>
        /// User-defined key/value metadata.
        /// Example: {&quot;com.example.some-label&quot;:&quot;some-value&quot;,&quot;com.example.some-other-label&quot;:&quot;some-other-value&quot;}
        /// Required: true
        /// </summary>
        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        /// <summary>
        /// Mount path of the volume on the host.
        /// Example: /var/lib/docker/volumes/tardis
        /// Required: true
        /// </summary>
        [JsonPropertyName("Mountpoint")]
        public string Mountpoint { get; set; } = default!;

        /// <summary>
        /// Name of the volume.
        /// Example: tardis
        /// Required: true
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// The driver specific options used when creating the volume.
        /// 
        /// Example: {&quot;device&quot;:&quot;tmpfs&quot;,&quot;o&quot;:&quot;size=100m,uid=1000&quot;,&quot;type&quot;:&quot;tmpfs&quot;}
        /// Required: true
        /// </summary>
        [JsonPropertyName("Options")]
        public IDictionary<string, string> Options { get; set; } = default!;

        /// <summary>
        /// The level at which the volume exists. Either `global` for cluster-wide,
        /// or `local` for machine level.
        /// 
        /// Example: local
        /// Required: true
        /// Enum: [&quot;local&quot;,&quot;global&quot;]
        /// </summary>
        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = default!;

        /// <summary>
        /// Low-level details about the volume, provided by the volume driver.
        /// Details are returned as a map with key/value pairs:
        /// `{&quot;key&quot;:&quot;value&quot;,&quot;key2&quot;:&quot;value2&quot;}`.
        /// 
        /// The `Status` field is optional, and is omitted if the volume driver
        /// does not support this feature.
        /// 
        /// Example: {&quot;hello&quot;:&quot;world&quot;}
        /// </summary>
        [JsonPropertyName("Status")]
        public IDictionary<string, object>? Status { get; set; }

        /// <summary>
        /// usage data
        /// </summary>
        [JsonPropertyName("UsageData")]
        public UsageData? UsageData { get; set; }
    }
}

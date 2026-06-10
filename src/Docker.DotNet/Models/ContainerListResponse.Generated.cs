#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Summary contains response of Engine API:
    /// GET &quot;/containers/json&quot;
    /// </summary>
    public class ContainerListResponse // (container.Summary)
    {
        [JsonPropertyName("Id")]
        public string ID { get; set; } = string.Empty;

        [JsonPropertyName("Names")]
        public IList<string> Names { get; set; } = default!;

        [JsonPropertyName("Image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("ImageID")]
        public string ImageID { get; set; } = string.Empty;

        [JsonPropertyName("ImageManifestDescriptor")]
        public Descriptor? ImageManifestDescriptor { get; set; }

        [JsonPropertyName("Command")]
        public string Command { get; set; } = string.Empty;

        [JsonPropertyName("Created")]
        public DateTime Created { get; set; } = default!;

        [JsonPropertyName("Ports")]
        public IList<PortSummary> Ports { get; set; } = default!;

        [JsonPropertyName("SizeRw")]
        public long? SizeRw { get; set; }

        [JsonPropertyName("SizeRootFs")]
        public long? SizeRootFs { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("State")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("Status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("HostConfig")]
        public SummaryHostConfig HostConfig { get; set; } = default!;

        [JsonPropertyName("Health")]
        public HealthSummary? Health { get; set; }

        [JsonPropertyName("NetworkSettings")]
        public NetworkSettingsSummary? NetworkSettings { get; set; }

        [JsonPropertyName("Mounts")]
        public IList<MountPoint> Mounts { get; set; } = default!;
    }
}

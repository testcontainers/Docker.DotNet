#nullable enable
namespace Docker.DotNet.Models
{
    public class SystemDataUsageInfoResponse // (system.DiskUsage)
    {
        [JsonPropertyName("ImageUsage")]
        public ImageDiskUsage? ImageUsage { get; set; }

        [JsonPropertyName("ContainerUsage")]
        public ContainerDiskUsage? ContainerUsage { get; set; }

        [JsonPropertyName("VolumeUsage")]
        public VolumeDiskUsage? VolumeUsage { get; set; }

        [JsonPropertyName("BuildCacheUsage")]
        public BuildDiskUsage? BuildCacheUsage { get; set; }
    }
}

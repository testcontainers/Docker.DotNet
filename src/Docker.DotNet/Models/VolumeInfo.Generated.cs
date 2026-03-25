#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeInfo // (volume.Info)
    {
        [JsonPropertyName("CapacityBytes")]
        public long CapacityBytes { get; set; } = default!;

        [JsonPropertyName("VolumeContext")]
        public IDictionary<string, string> VolumeContext { get; set; } = default!;

        [JsonPropertyName("VolumeID")]
        public string VolumeID { get; set; } = default!;

        [JsonPropertyName("AccessibleTopology")]
        public IList<VolumeTopology> AccessibleTopology { get; set; } = default!;
    }
}

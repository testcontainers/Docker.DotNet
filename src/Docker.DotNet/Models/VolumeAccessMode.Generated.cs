#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumeAccessMode // (volume.AccessMode)
    {
        [JsonPropertyName("Scope")]
        public string Scope { get; set; } = default!;

        [JsonPropertyName("Sharing")]
        public string Sharing { get; set; } = default!;

        [JsonPropertyName("MountVolume")]
        public TypeMount? MountVolume { get; set; }

        [JsonPropertyName("BlockVolume")]
        public TypeBlock? BlockVolume { get; set; }
    }
}

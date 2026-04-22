#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// AccessMode defines the access mode of a volume.
    /// </summary>
    public class VolumeAccessMode // (volume.AccessMode)
    {
        /// <summary>
        /// Scope defines the set of nodes this volume can be used on at one time.
        /// </summary>
        [JsonPropertyName("Scope")]
        public string? Scope { get; set; }

        /// <summary>
        /// Sharing defines the number and way that different tasks can use this
        /// volume at one time.
        /// </summary>
        [JsonPropertyName("Sharing")]
        public string? Sharing { get; set; }

        /// <summary>
        /// MountVolume defines options for using this volume as a Mount-type
        /// volume.
        /// 
        /// Either BlockVolume or MountVolume, but not both, must be present.
        /// </summary>
        [JsonPropertyName("MountVolume")]
        public TypeMount? MountVolume { get; set; }

        /// <summary>
        /// BlockVolume defines options for using this volume as a Block-type
        /// volume.
        /// 
        /// Either BlockVolume or MountVolume, but not both, must be present.
        /// </summary>
        [JsonPropertyName("BlockVolume")]
        public TypeBlock? BlockVolume { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// TypeMount contains options for using a volume as a Mount-type
    /// volume.
    /// </summary>
    public class TypeMount // (volume.TypeMount)
    {
        /// <summary>
        /// FsType specifies the filesystem type for the mount volume. Optional.
        /// </summary>
        [JsonPropertyName("FsType")]
        public string? FsType { get; set; }

        /// <summary>
        /// MountFlags defines flags to pass when mounting the volume. Optional.
        /// </summary>
        [JsonPropertyName("MountFlags")]
        public IList<string>? MountFlags { get; set; }
    }
}

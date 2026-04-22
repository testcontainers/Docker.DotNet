#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// TmpfsOptions defines options specific to mounts of type &quot;tmpfs&quot;.
    /// </summary>
    public class TmpfsOptions // (mount.TmpfsOptions)
    {
        /// <summary>
        /// Size sets the size of the tmpfs, in bytes.
        /// 
        /// This will be converted to an operating system specific value
        /// depending on the host. For example, on linux, it will be converted to
        /// use a &apos;k&apos;, &apos;m&apos; or &apos;g&apos; syntax. BSD, though not widely supported with
        /// docker, uses a straight byte value.
        /// 
        /// Percentages are not supported.
        /// </summary>
        [JsonPropertyName("SizeBytes")]
        public long? SizeBytes { get; set; }

        /// <summary>
        /// Mode of the tmpfs upon creation
        /// </summary>
        [JsonPropertyName("Mode")]
        public uint? Mode { get; set; }

        /// <summary>
        /// Options to be passed to the tmpfs mount. An array of arrays. Flag
        /// options should be provided as 1-length arrays. Other types should be
        /// provided as 2-length arrays, where the first item is the key and the
        /// second the value.
        /// </summary>
        [JsonPropertyName("Options")]
        public IList<IList<string>>? Options { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// BindOptions defines options specific to mounts of type &quot;bind&quot;.
    /// </summary>
    public class BindOptions // (mount.BindOptions)
    {
        [JsonPropertyName("Propagation")]
        public string? Propagation { get; set; }

        [JsonPropertyName("NonRecursive")]
        public bool? NonRecursive { get; set; }

        [JsonPropertyName("CreateMountpoint")]
        public bool? CreateMountpoint { get; set; }

        /// <summary>
        /// ReadOnlyNonRecursive makes the mount non-recursively read-only, but still leaves the mount recursive
        /// (unless NonRecursive is set to true in conjunction).
        /// </summary>
        [JsonPropertyName("ReadOnlyNonRecursive")]
        public bool? ReadOnlyNonRecursive { get; set; }

        /// <summary>
        /// ReadOnlyForceRecursive raises an error if the mount cannot be made recursively read-only.
        /// </summary>
        [JsonPropertyName("ReadOnlyForceRecursive")]
        public bool? ReadOnlyForceRecursive { get; set; }
    }
}

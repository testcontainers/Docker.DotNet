#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// MountPoint represents a mount point configuration inside the container.
    /// This is used for reporting the mountpoints in use by a container.
    /// </summary>
    public class MountPoint // (container.MountPoint)
    {
        /// <summary>
        /// Type is the type of mount, see [mount.Type] definitions for details.
        /// </summary>
        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        /// <summary>
        /// Name is the name reference to the underlying data defined by `Source`
        /// e.g., the volume name.
        /// </summary>
        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        /// <summary>
        /// Source is the source location of the mount.
        /// 
        /// For volumes, this contains the storage location of the volume (within
        /// `/var/lib/docker/volumes/`). For bind-mounts, and `npipe`, this contains
        /// the source (host) part of the bind-mount. For `tmpfs` mount points, this
        /// field is empty.
        /// </summary>
        [JsonPropertyName("Source")]
        public string Source { get; set; } = default!;

        /// <summary>
        /// Destination is the path relative to the container root (`/`) where the
        /// Source is mounted inside the container.
        /// </summary>
        [JsonPropertyName("Destination")]
        public string Destination { get; set; } = default!;

        /// <summary>
        /// Driver is the volume driver used to create the volume (if it is a volume).
        /// </summary>
        [JsonPropertyName("Driver")]
        public string? Driver { get; set; }

        /// <summary>
        /// Mode is a comma separated list of options supplied by the user when
        /// creating the bind/volume mount.
        /// 
        /// The default is platform-specific (`&quot;z&quot;` on Linux, empty on Windows).
        /// </summary>
        [JsonPropertyName("Mode")]
        public string Mode { get; set; } = default!;

        /// <summary>
        /// RW indicates whether the mount is mounted writable (read-write).
        /// </summary>
        [JsonPropertyName("RW")]
        public bool RW { get; set; } = default!;

        /// <summary>
        /// Propagation describes how mounts are propagated from the host into the
        /// mount point, and vice-versa. Refer to the Linux kernel documentation
        /// for details:
        /// https://www.kernel.org/doc/Documentation/filesystems/sharedsubtree.txt
        /// 
        /// This field is not used on Windows.
        /// </summary>
        [JsonPropertyName("Propagation")]
        public string Propagation { get; set; } = default!;
    }
}

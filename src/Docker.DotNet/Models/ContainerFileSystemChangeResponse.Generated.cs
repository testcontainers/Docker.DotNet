#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// FilesystemChange Change in the container&apos;s filesystem.
    /// 
    /// swagger:model FilesystemChange
    /// </summary>
    public class ContainerFileSystemChangeResponse // (container.FilesystemChange)
    {
        /// <summary>
        /// kind
        /// Required: true
        /// </summary>
        [JsonPropertyName("Kind")]
        public FileSystemChangeKind Kind { get; set; } = default!;

        /// <summary>
        /// Path to file or directory that has changed.
        /// 
        /// Required: true
        /// </summary>
        [JsonPropertyName("Path")]
        public string Path { get; set; } = default!;
    }
}

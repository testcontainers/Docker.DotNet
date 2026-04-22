#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RootFSStorageSnapshot Information about a snapshot backend of the container&apos;s root filesystem.
    /// 
    /// swagger:model RootFSStorageSnapshot
    /// </summary>
    public class RootFSStorageSnapshot // (storage.RootFSStorageSnapshot)
    {
        /// <summary>
        /// Name of the snapshotter.
        /// </summary>
        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}

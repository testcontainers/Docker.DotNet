#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RootFSStorage Information about the storage used for the container&apos;s root filesystem.
    /// 
    /// swagger:model RootFSStorage
    /// </summary>
    public class RootFSStorage // (storage.RootFSStorage)
    {
        /// <summary>
        /// Information about the snapshot used for the container&apos;s root filesystem.
        /// </summary>
        [JsonPropertyName("Snapshot")]
        public RootFSStorageSnapshot? Snapshot { get; set; }
    }
}

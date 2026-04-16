#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Storage Information about the storage used by the container.
    /// 
    /// swagger:model Storage
    /// </summary>
    public class Storage // (storage.Storage)
    {
        /// <summary>
        /// Information about the storage used for the container&apos;s root filesystem.
        /// </summary>
        [JsonPropertyName("RootFS")]
        public RootFSStorage? RootFS { get; set; }
    }
}

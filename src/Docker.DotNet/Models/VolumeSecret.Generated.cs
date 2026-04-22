#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Secret represents a Swarm Secret value that must be passed to the CSI
    /// storage plugin when operating on this Volume. It represents one key-value
    /// pair of possibly many.
    /// </summary>
    public class VolumeSecret // (volume.Secret)
    {
        /// <summary>
        /// Key is the name of the key of the key-value pair passed to the plugin.
        /// </summary>
        [JsonPropertyName("Key")]
        public string Key { get; set; } = default!;

        /// <summary>
        /// Secret is the swarm Secret object from which to read data. This can be a
        /// Secret name or ID. The Secret data is retrieved by Swarm and used as the
        /// value of the key-value pair passed to the plugin.
        /// </summary>
        [JsonPropertyName("Secret")]
        public string Secret { get; set; } = default!;
    }
}

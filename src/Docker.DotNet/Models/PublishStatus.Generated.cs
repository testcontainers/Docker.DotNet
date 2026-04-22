#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PublishStatus represents the status of the volume as published to an
    /// individual node
    /// </summary>
    public class PublishStatus // (volume.PublishStatus)
    {
        /// <summary>
        /// NodeID is the ID of the swarm node this Volume is published to.
        /// </summary>
        [JsonPropertyName("NodeID")]
        public string? NodeID { get; set; }

        /// <summary>
        /// State is the publish state of the volume.
        /// </summary>
        [JsonPropertyName("State")]
        public string? State { get; set; }

        /// <summary>
        /// PublishContext is the PublishContext returned by the CSI plugin when
        /// a volume is published.
        /// </summary>
        [JsonPropertyName("PublishContext")]
        public IDictionary<string, string>? PublishContext { get; set; }
    }
}

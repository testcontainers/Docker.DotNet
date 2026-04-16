#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// VolumeAttachment contains the associating a Volume to a Task.
    /// </summary>
    public class VolumeAttachment // (swarm.VolumeAttachment)
    {
        /// <summary>
        /// ID is the Swarmkit ID of the Volume. This is not the CSI VolumeId.
        /// </summary>
        [JsonPropertyName("ID")]
        public string? ID { get; set; }

        /// <summary>
        /// Source, together with Target, indicates the Mount, as specified in the
        /// ContainerSpec, that this volume fulfills.
        /// </summary>
        [JsonPropertyName("Source")]
        public string? Source { get; set; }

        /// <summary>
        /// Target, together with Source, indicates the Mount, as specified
        /// in the ContainerSpec, that this volume fulfills.
        /// </summary>
        [JsonPropertyName("Target")]
        public string? Target { get; set; }
    }
}

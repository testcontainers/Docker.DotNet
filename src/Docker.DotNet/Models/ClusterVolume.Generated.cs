#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ClusterVolume contains options and information specific to, and only present
    /// on, Swarm CSI cluster volumes.
    /// </summary>
    public class ClusterVolume // (volume.ClusterVolume)
    {
        public ClusterVolume()
        {
        }

        public ClusterVolume(Meta Meta)
        {
            if (Meta != null)
            {
                this.Version = Meta.Version;
                this.CreatedAt = Meta.CreatedAt;
                this.UpdatedAt = Meta.UpdatedAt;
            }
        }

        /// <summary>
        /// ID is the Swarm ID of the volume. Because cluster volumes are Swarm
        /// objects, they have an ID, unlike non-cluster volumes, which only have a
        /// Name. This ID can be used to refer to the cluster volume.
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Version")]
        public Version? Version { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Spec is the cluster-specific options from which this volume is derived.
        /// </summary>
        [JsonPropertyName("Spec")]
        public ClusterVolumeSpec Spec { get; set; } = default!;

        /// <summary>
        /// PublishStatus contains the status of the volume as it pertains to its
        /// publishing on Nodes.
        /// </summary>
        [JsonPropertyName("PublishStatus")]
        public IList<PublishStatus>? PublishStatus { get; set; }

        /// <summary>
        /// Info is information about the global status of the volume.
        /// </summary>
        [JsonPropertyName("Info")]
        public VolumeInfo? Info { get; set; }
    }
}

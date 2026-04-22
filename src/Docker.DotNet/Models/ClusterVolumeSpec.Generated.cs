#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ClusterVolumeSpec contains the spec used to create this volume.
    /// </summary>
    public class ClusterVolumeSpec // (volume.ClusterVolumeSpec)
    {
        /// <summary>
        /// Group defines the volume group of this volume. Volumes belonging to the
        /// same group can be referred to by group name when creating Services.
        /// Referring to a volume by group instructs swarm to treat volumes in that
        /// group interchangeably for the purpose of scheduling. Volumes with an
        /// empty string for a group technically all belong to the same, emptystring
        /// group.
        /// </summary>
        [JsonPropertyName("Group")]
        public string? Group { get; set; }

        /// <summary>
        /// AccessMode defines how the volume is used by tasks.
        /// </summary>
        [JsonPropertyName("AccessMode")]
        public VolumeAccessMode? AccessMode { get; set; }

        /// <summary>
        /// AccessibilityRequirements specifies where in the cluster a volume must
        /// be accessible from.
        /// 
        /// This field must be empty if the plugin does not support
        /// VOLUME_ACCESSIBILITY_CONSTRAINTS capabilities. If it is present but the
        /// plugin does not support it, volume will not be created.
        /// 
        /// If AccessibilityRequirements is empty, but the plugin does support
        /// VOLUME_ACCESSIBILITY_CONSTRAINTS, then Swarmkit will assume the entire
        /// cluster is a valid target for the volume.
        /// </summary>
        [JsonPropertyName("AccessibilityRequirements")]
        public TopologyRequirement? AccessibilityRequirements { get; set; }

        /// <summary>
        /// CapacityRange defines the desired capacity that the volume should be
        /// created with. If nil, the plugin will decide the capacity.
        /// </summary>
        [JsonPropertyName("CapacityRange")]
        public CapacityRange? CapacityRange { get; set; }

        /// <summary>
        /// Secrets defines Swarm Secrets that are passed to the CSI storage plugin
        /// when operating on this volume.
        /// </summary>
        [JsonPropertyName("Secrets")]
        public IList<VolumeSecret>? Secrets { get; set; }

        /// <summary>
        /// Availability is the Volume&apos;s desired availability. Analogous to Node
        /// Availability, this allows the user to take volumes offline in order to
        /// update or delete them.
        /// </summary>
        [JsonPropertyName("Availability")]
        public string? Availability { get; set; }
    }
}

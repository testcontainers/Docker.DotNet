#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Info contains information about the Volume as a whole as provided by
    /// the CSI storage plugin.
    /// </summary>
    public class VolumeInfo // (volume.Info)
    {
        /// <summary>
        /// CapacityBytes is the capacity of the volume in bytes. A value of 0
        /// indicates that the capacity is unknown.
        /// </summary>
        [JsonPropertyName("CapacityBytes")]
        public long? CapacityBytes { get; set; }

        /// <summary>
        /// VolumeContext is the context originating from the CSI storage plugin
        /// when the Volume is created.
        /// </summary>
        [JsonPropertyName("VolumeContext")]
        public IDictionary<string, string>? VolumeContext { get; set; }

        /// <summary>
        /// VolumeID is the ID of the Volume as seen by the CSI storage plugin. This
        /// is distinct from the Volume&apos;s Swarm ID, which is the ID used by all of
        /// the Docker Engine to refer to the Volume. If this field is blank, then
        /// the Volume has not been successfully created yet.
        /// </summary>
        [JsonPropertyName("VolumeID")]
        public string? VolumeID { get; set; }

        /// <summary>
        /// AccessibleTopology is the topology this volume is actually accessible
        /// from.
        /// </summary>
        [JsonPropertyName("AccessibleTopology")]
        public IList<VolumeTopology>? AccessibleTopology { get; set; }
    }
}

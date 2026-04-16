#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CapacityRange describes the minimum and maximum capacity a volume should be
    /// created with
    /// </summary>
    public class CapacityRange // (volume.CapacityRange)
    {
        /// <summary>
        /// RequiredBytes specifies that a volume must be at least this big. The
        /// value of 0 indicates an unspecified minimum.
        /// </summary>
        [JsonPropertyName("RequiredBytes")]
        public long RequiredBytes { get; set; } = default!;

        /// <summary>
        /// LimitBytes specifies that a volume must not be bigger than this. The
        /// value of 0 indicates an unspecified maximum
        /// </summary>
        [JsonPropertyName("LimitBytes")]
        public long LimitBytes { get; set; } = default!;
    }
}

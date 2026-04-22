#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ThrottlingData stores CPU throttling stats of one running container.
    /// Not used on Windows.
    /// </summary>
    public class ThrottlingData // (container.ThrottlingData)
    {
        /// <summary>
        /// Number of periods with throttling active
        /// </summary>
        [JsonPropertyName("periods")]
        public ulong Periods { get; set; } = default!;

        /// <summary>
        /// Number of periods when the container hits its throttling limit.
        /// </summary>
        [JsonPropertyName("throttled_periods")]
        public ulong ThrottledPeriods { get; set; } = default!;

        /// <summary>
        /// Aggregate time the container was throttled for in nanoseconds.
        /// </summary>
        [JsonPropertyName("throttled_time")]
        public ulong ThrottledTime { get; set; } = default!;
    }
}

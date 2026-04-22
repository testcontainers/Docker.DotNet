#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// HealthSummary stores a summary of the container&apos;s healthcheck results.
    /// </summary>
    public class HealthSummary // (container.HealthSummary)
    {
        /// <summary>
        /// Status is one of [NoHealthcheck], [Starting], [Healthy] or [Unhealthy].
        /// </summary>
        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;

        /// <summary>
        /// FailingStreak is the number of consecutive failures
        /// </summary>
        [JsonPropertyName("FailingStreak")]
        public long FailingStreak { get; set; } = default!;
    }
}

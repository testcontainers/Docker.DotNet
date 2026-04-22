#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Health stores information about the container&apos;s healthcheck results
    /// </summary>
    public class Health // (container.Health)
    {
        /// <summary>
        /// Status is one of [Starting], [Healthy] or [Unhealthy].
        /// </summary>
        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;

        /// <summary>
        /// FailingStreak is the number of consecutive failures
        /// </summary>
        [JsonPropertyName("FailingStreak")]
        public long FailingStreak { get; set; } = default!;

        /// <summary>
        /// Log contains the last few results (oldest first)
        /// </summary>
        [JsonPropertyName("Log")]
        public IList<HealthcheckResult> Log { get; set; } = default!;
    }
}

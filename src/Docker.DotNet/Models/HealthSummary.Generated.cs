#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// HealthSummary stores a summary of the container&apos;s healthcheck results.
    /// </summary>
    public class HealthSummary // (container.HealthSummary)
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("FailingStreak")]
        public long FailingStreak { get; set; } = default!;
    }
}

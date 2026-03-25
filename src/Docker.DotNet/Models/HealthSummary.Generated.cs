#nullable enable
namespace Docker.DotNet.Models
{
    public class HealthSummary // (container.HealthSummary)
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("FailingStreak")]
        public long FailingStreak { get; set; } = default!;
    }
}

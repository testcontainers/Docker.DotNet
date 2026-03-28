#nullable enable
namespace Docker.DotNet.Models
{
    public class Health // (container.Health)
    {
        [JsonPropertyName("Status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("FailingStreak")]
        public long FailingStreak { get; set; } = default!;

        [JsonPropertyName("Log")]
        public IList<HealthcheckResult> Log { get; set; } = default!;
    }
}

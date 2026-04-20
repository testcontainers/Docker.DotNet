#nullable enable
namespace Docker.DotNet.Models
{
    public class HealthcheckConfig // (v1.HealthcheckConfig)
    {
        [JsonPropertyName("Test")]
        public IList<string>? Test { get; set; }

        [JsonPropertyName("Interval")]
        public TimeSpan? Interval { get; set; }

        [JsonPropertyName("Timeout")]
        public TimeSpan? Timeout { get; set; }

        [JsonPropertyName("StartPeriod")]
        public TimeSpan? StartPeriod { get; set; }

        [JsonPropertyName("StartInterval")]
        public TimeSpan? StartInterval { get; set; }

        [JsonPropertyName("Retries")]
        public long? Retries { get; set; }
    }
}

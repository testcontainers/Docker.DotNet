#nullable enable
namespace Docker.DotNet.Models
{
    public class HealthcheckConfig // (v1.HealthcheckConfig)
    {
        [JsonPropertyName("Test")]
        public IList<string> Test { get; set; } = default!;

        [JsonPropertyName("Interval")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan Interval { get; set; } = default!;

        [JsonPropertyName("Timeout")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan Timeout { get; set; } = default!;

        [JsonPropertyName("StartPeriod")]
        public long StartPeriod { get; set; } = default!;

        [JsonPropertyName("StartInterval")]
        public long StartInterval { get; set; } = default!;

        [JsonPropertyName("Retries")]
        public long Retries { get; set; } = default!;
    }
}

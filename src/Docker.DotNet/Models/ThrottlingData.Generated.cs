#nullable enable
namespace Docker.DotNet.Models
{
    public class ThrottlingData // (container.ThrottlingData)
    {
        [JsonPropertyName("periods")]
        public ulong Periods { get; set; } = default!;

        [JsonPropertyName("throttled_periods")]
        public ulong ThrottledPeriods { get; set; } = default!;

        [JsonPropertyName("throttled_time")]
        public ulong ThrottledTime { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class DispatcherConfig // (swarm.DispatcherConfig)
    {
        [JsonPropertyName("HeartbeatPeriod")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan? HeartbeatPeriod { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class DispatcherConfig // (swarm.DispatcherConfig)
    {
        [JsonPropertyName("HeartbeatPeriod")]
        public TimeSpan? HeartbeatPeriod { get; set; }
    }
}

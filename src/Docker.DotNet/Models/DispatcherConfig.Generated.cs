namespace Docker.DotNet.Models
{
    public class DispatcherConfig // (swarm.DispatcherConfig)
    {
        [JsonPropertyName("HeartbeatPeriod")]
        public long HeartbeatPeriod { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DispatcherConfig represents dispatcher configuration.
    /// </summary>
    public class DispatcherConfig // (swarm.DispatcherConfig)
    {
        /// <summary>
        /// HeartbeatPeriod defines how often agent should send heartbeats to
        /// dispatcher.
        /// </summary>
        [JsonPropertyName("HeartbeatPeriod")]
        public TimeSpan? HeartbeatPeriod { get; set; }
    }
}

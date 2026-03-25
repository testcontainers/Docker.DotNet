#nullable enable
namespace Docker.DotNet.Models
{
    public class RaftConfig // (swarm.RaftConfig)
    {
        [JsonPropertyName("SnapshotInterval")]
        public ulong SnapshotInterval { get; set; } = default!;

        [JsonPropertyName("KeepOldSnapshots")]
        public ulong? KeepOldSnapshots { get; set; }

        [JsonPropertyName("LogEntriesForSlowFollowers")]
        public ulong LogEntriesForSlowFollowers { get; set; } = default!;

        [JsonPropertyName("ElectionTick")]
        public long ElectionTick { get; set; } = default!;

        [JsonPropertyName("HeartbeatTick")]
        public long HeartbeatTick { get; set; } = default!;
    }
}

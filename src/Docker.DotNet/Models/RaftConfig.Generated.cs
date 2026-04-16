#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RaftConfig represents raft configuration.
    /// </summary>
    public class RaftConfig // (swarm.RaftConfig)
    {
        /// <summary>
        /// SnapshotInterval is the number of log entries between snapshots.
        /// </summary>
        [JsonPropertyName("SnapshotInterval")]
        public ulong? SnapshotInterval { get; set; }

        /// <summary>
        /// KeepOldSnapshots is the number of snapshots to keep beyond the
        /// current snapshot.
        /// </summary>
        [JsonPropertyName("KeepOldSnapshots")]
        public ulong? KeepOldSnapshots { get; set; }

        /// <summary>
        /// LogEntriesForSlowFollowers is the number of log entries to keep
        /// around to sync up slow followers after a snapshot is created.
        /// </summary>
        [JsonPropertyName("LogEntriesForSlowFollowers")]
        public ulong? LogEntriesForSlowFollowers { get; set; }

        /// <summary>
        /// ElectionTick is the number of ticks that a follower will wait for a message
        /// from the leader before becoming a candidate and starting an election.
        /// ElectionTick must be greater than HeartbeatTick.
        /// 
        /// A tick currently defaults to one second, so these translate directly to
        /// seconds currently, but this is NOT guaranteed.
        /// </summary>
        [JsonPropertyName("ElectionTick")]
        public long ElectionTick { get; set; } = default!;

        /// <summary>
        /// HeartbeatTick is the number of ticks between heartbeats. Every
        /// HeartbeatTick ticks, the leader will send a heartbeat to the
        /// followers.
        /// 
        /// A tick currently defaults to one second, so these translate directly to
        /// seconds currently, but this is NOT guaranteed.
        /// </summary>
        [JsonPropertyName("HeartbeatTick")]
        public long HeartbeatTick { get; set; } = default!;
    }
}

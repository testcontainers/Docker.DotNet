#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkStats aggregates the network stats of one container
    /// </summary>
    public class NetworkStats // (container.NetworkStats)
    {
        /// <summary>
        /// Bytes received. Windows and Linux.
        /// </summary>
        [JsonPropertyName("rx_bytes")]
        public ulong RxBytes { get; set; } = default!;

        /// <summary>
        /// Packets received. Windows and Linux.
        /// </summary>
        [JsonPropertyName("rx_packets")]
        public ulong RxPackets { get; set; } = default!;

        /// <summary>
        /// Received errors. Not used on Windows. Note that we don&apos;t `omitempty` this
        /// field as it is expected in the &gt;=v1.21 API stats structure.
        /// </summary>
        [JsonPropertyName("rx_errors")]
        public ulong RxErrors { get; set; } = default!;

        /// <summary>
        /// Incoming packets dropped. Windows and Linux.
        /// </summary>
        [JsonPropertyName("rx_dropped")]
        public ulong RxDropped { get; set; } = default!;

        /// <summary>
        /// Bytes sent. Windows and Linux.
        /// </summary>
        [JsonPropertyName("tx_bytes")]
        public ulong TxBytes { get; set; } = default!;

        /// <summary>
        /// Packets sent. Windows and Linux.
        /// </summary>
        [JsonPropertyName("tx_packets")]
        public ulong TxPackets { get; set; } = default!;

        /// <summary>
        /// Sent errors. Not used on Windows. Note that we don&apos;t `omitempty` this
        /// field as it is expected in the &gt;=v1.21 API stats structure.
        /// </summary>
        [JsonPropertyName("tx_errors")]
        public ulong TxErrors { get; set; } = default!;

        /// <summary>
        /// Outgoing packets dropped. Windows and Linux.
        /// </summary>
        [JsonPropertyName("tx_dropped")]
        public ulong TxDropped { get; set; } = default!;

        /// <summary>
        /// Endpoint ID. Not used on Linux.
        /// </summary>
        [JsonPropertyName("endpoint_id")]
        public string? EndpointID { get; set; }

        /// <summary>
        /// Instance ID. Not used on Linux.
        /// </summary>
        [JsonPropertyName("instance_id")]
        public string? InstanceID { get; set; }
    }
}

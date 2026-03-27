#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworkStats // (container.NetworkStats)
    {
        [JsonPropertyName("rx_bytes")]
        public ulong RxBytes { get; set; } = default!;

        [JsonPropertyName("rx_packets")]
        public ulong RxPackets { get; set; } = default!;

        [JsonPropertyName("rx_errors")]
        public ulong RxErrors { get; set; } = default!;

        [JsonPropertyName("rx_dropped")]
        public ulong RxDropped { get; set; } = default!;

        [JsonPropertyName("tx_bytes")]
        public ulong TxBytes { get; set; } = default!;

        [JsonPropertyName("tx_packets")]
        public ulong TxPackets { get; set; } = default!;

        [JsonPropertyName("tx_errors")]
        public ulong TxErrors { get; set; } = default!;

        [JsonPropertyName("tx_dropped")]
        public ulong TxDropped { get; set; } = default!;

        [JsonPropertyName("endpoint_id")]
        public string? EndpointID { get; set; }

        [JsonPropertyName("instance_id")]
        public string? InstanceID { get; set; }
    }
}

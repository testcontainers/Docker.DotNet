#nullable enable
namespace Docker.DotNet.Models
{
    public class StorageStats // (container.StorageStats)
    {
        [JsonPropertyName("read_count_normalized")]
        public ulong ReadCountNormalized { get; set; } = default!;

        [JsonPropertyName("read_size_bytes")]
        public ulong ReadSizeBytes { get; set; } = default!;

        [JsonPropertyName("write_count_normalized")]
        public ulong WriteCountNormalized { get; set; } = default!;

        [JsonPropertyName("write_size_bytes")]
        public ulong WriteSizeBytes { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// StorageStats is the disk I/O stats for read/write on Windows.
    /// </summary>
    public class StorageStats // (container.StorageStats)
    {
        [JsonPropertyName("read_count_normalized")]
        public ulong? ReadCountNormalized { get; set; }

        [JsonPropertyName("read_size_bytes")]
        public ulong? ReadSizeBytes { get; set; }

        [JsonPropertyName("write_count_normalized")]
        public ulong? WriteCountNormalized { get; set; }

        [JsonPropertyName("write_size_bytes")]
        public ulong? WriteSizeBytes { get; set; }
    }
}

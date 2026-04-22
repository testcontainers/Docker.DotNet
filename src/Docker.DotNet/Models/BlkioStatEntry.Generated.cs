#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// BlkioStatEntry is one small entity to store a piece of Blkio stats
    /// Not used on Windows.
    /// </summary>
    public class BlkioStatEntry // (container.BlkioStatEntry)
    {
        [JsonPropertyName("major")]
        public ulong Major { get; set; } = default!;

        [JsonPropertyName("minor")]
        public ulong Minor { get; set; } = default!;

        [JsonPropertyName("op")]
        public string Op { get; set; } = default!;

        [JsonPropertyName("value")]
        public ulong Value { get; set; } = default!;
    }
}

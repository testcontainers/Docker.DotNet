#nullable enable
namespace Docker.DotNet.Models
{
    public class CacheRecord // (build.CacheRecord)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        [JsonPropertyName(" Parents")]
        public IList<string>? Parents { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("InUse")]
        public bool InUse { get; set; } = default!;

        [JsonPropertyName("Shared")]
        public bool Shared { get; set; } = default!;

        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;

        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; } = default!;

        [JsonPropertyName("LastUsedAt")]
        public DateTime? LastUsedAt { get; set; }

        [JsonPropertyName("UsageCount")]
        public long UsageCount { get; set; } = default!;
    }
}

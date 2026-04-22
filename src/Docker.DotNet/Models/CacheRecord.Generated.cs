#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// CacheRecord contains information about a build cache record.
    /// </summary>
    public class CacheRecord // (build.CacheRecord)
    {
        /// <summary>
        /// ID is the unique ID of the build cache record.
        /// </summary>
        [JsonPropertyName("ID")]
        public string ID { get; set; } = default!;

        /// <summary>
        /// Parents is the list of parent build cache record IDs.
        /// </summary>
        [JsonPropertyName(" Parents")]
        public IList<string>? Parents { get; set; }

        /// <summary>
        /// Type is the cache record type.
        /// </summary>
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        /// <summary>
        /// Description is a description of the build-step that produced the build cache.
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        /// <summary>
        /// InUse indicates if the build cache is in use.
        /// </summary>
        [JsonPropertyName("InUse")]
        public bool InUse { get; set; } = default!;

        /// <summary>
        /// Shared indicates if the build cache is shared.
        /// </summary>
        [JsonPropertyName("Shared")]
        public bool Shared { get; set; } = default!;

        /// <summary>
        /// Size is the amount of disk space used by the build cache (in bytes).
        /// </summary>
        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;

        /// <summary>
        /// CreatedAt is the date and time at which the build cache was created.
        /// </summary>
        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; } = default!;

        /// <summary>
        /// LastUsedAt is the date and time at which the build cache was last used.
        /// </summary>
        [JsonPropertyName("LastUsedAt")]
        public DateTime? LastUsedAt { get; set; }

        [JsonPropertyName("UsageCount")]
        public long UsageCount { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SearchResult describes a search result returned from a registry
    /// </summary>
    public class ImageSearchResponse // (registry.SearchResult)
    {
        /// <summary>
        /// StarCount indicates the number of stars this repository has
        /// </summary>
        [JsonPropertyName("star_count")]
        public long StarCount { get; set; } = default!;

        /// <summary>
        /// IsOfficial is true if the result is from an official repository.
        /// </summary>
        [JsonPropertyName("is_official")]
        public bool IsOfficial { get; set; } = default!;

        /// <summary>
        /// Name is the name of the repository
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// IsAutomated indicates whether the result is automated.
        /// 
        /// Deprecated: the &quot;is_automated&quot; field is deprecated and will always be &quot;false&quot;.
        /// </summary>
        [JsonPropertyName("is_automated")]
        public bool IsAutomated { get; set; } = default!;

        /// <summary>
        /// Description is a textual description of the repository
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;
    }
}

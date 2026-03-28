#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageSearchResponse // (registry.SearchResult)
    {
        [JsonPropertyName("star_count")]
        public long StarCount { get; set; } = default!;

        [JsonPropertyName("is_official")]
        public bool IsOfficial { get; set; } = default!;

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("is_automated")]
        public bool IsAutomated { get; set; } = default!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;
    }
}

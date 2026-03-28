#nullable enable
namespace Docker.DotNet.Models
{
    public class IndexInfo // (registry.IndexInfo)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Mirrors")]
        public IList<string> Mirrors { get; set; } = default!;

        [JsonPropertyName("Secure")]
        public bool Secure { get; set; } = default!;

        [JsonPropertyName("Official")]
        public bool Official { get; set; } = default!;
    }
}

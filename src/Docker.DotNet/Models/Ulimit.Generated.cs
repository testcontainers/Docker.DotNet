#nullable enable
namespace Docker.DotNet.Models
{
    public class Ulimit // (units.Ulimit)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("Hard")]
        public long Hard { get; set; } = default!;

        [JsonPropertyName("Soft")]
        public long Soft { get; set; } = default!;
    }
}

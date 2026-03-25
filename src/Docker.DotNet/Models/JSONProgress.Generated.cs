#nullable enable
namespace Docker.DotNet.Models
{
    public class JSONProgress // (jsonstream.Progress)
    {
        [JsonPropertyName("current")]
        public long Current { get; set; } = default!;

        [JsonPropertyName("total")]
        public long Total { get; set; } = default!;

        [JsonPropertyName("start")]
        public long Start { get; set; } = default!;

        [JsonPropertyName("hidecounts")]
        public bool HideCounts { get; set; } = default!;

        [JsonPropertyName("units")]
        public string Units { get; set; } = default!;
    }
}

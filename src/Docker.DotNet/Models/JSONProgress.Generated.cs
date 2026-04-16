#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Progress describes a progress message in a JSON stream.
    /// </summary>
    public class JSONProgress // (jsonstream.Progress)
    {
        [JsonPropertyName("current")]
        public long? Current { get; set; }

        [JsonPropertyName("total")]
        public long? Total { get; set; }

        [JsonPropertyName("start")]
        public long? Start { get; set; }

        [JsonPropertyName("hidecounts")]
        public bool? HideCounts { get; set; }

        [JsonPropertyName("units")]
        public string? Units { get; set; }
    }
}

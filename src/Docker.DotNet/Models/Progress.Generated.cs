namespace Docker.DotNet.Models
{
    public class Progress // (jsonstream.Progress)
    {
        [JsonPropertyName("current")]
        public long Current { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("start")]
        public long Start { get; set; }

        [JsonPropertyName("hidecounts")]
        public bool HideCounts { get; set; }

        [JsonPropertyName("units")]
        public string Units { get; set; }
    }
}

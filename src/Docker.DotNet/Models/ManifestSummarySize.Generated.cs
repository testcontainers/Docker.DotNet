namespace Docker.DotNet.Models
{
    public class ManifestSummarySize // (image.ManifestSummary.Size)
    {
        [JsonPropertyName("Content")]
        public long Content { get; set; }

        [JsonPropertyName("Total")]
        public long Total { get; set; }
    }
}

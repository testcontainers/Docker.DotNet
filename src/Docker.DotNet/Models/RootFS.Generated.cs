namespace Docker.DotNet.Models
{
    public class RootFS // (image.RootFS)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Layers")]
        public IList<string> Layers { get; set; }
    }
}

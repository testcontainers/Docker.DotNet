namespace Docker.DotNet.Models
{
    public class Commit // (system.Commit)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("Expected")]
        public string Expected { get; set; }
    }
}

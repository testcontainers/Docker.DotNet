namespace Docker.DotNet.Models
{
    public class JSONMessage // (jsonstream.Message)
    {
        [JsonPropertyName("stream")]
        public string Stream { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("progressDetail")]
        public Progress Progress { get; set; }

        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("errorDetail")]
        public JSONError Error { get; set; }

        [JsonPropertyName("aux")]
        public ObjectExtensionData Aux { get; set; }
    }
}

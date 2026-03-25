#nullable enable
namespace Docker.DotNet.Models
{
    public class JSONMessage // (jsonstream.Message)
    {
        [JsonPropertyName("stream")]
        public string Stream { get; set; } = default!;

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("progressDetail")]
        public JSONProgress? Progress { get; set; }

        [JsonPropertyName("id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("errorDetail")]
        public JSONError? Error { get; set; }

        [JsonPropertyName("aux")]
        public ObjectExtensionData? Aux { get; set; }
    }
}

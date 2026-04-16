#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// JSONMessage defines a message struct. It describes
    /// the created time, where it from, status, ID of the
    /// message. It&apos;s used for docker events.
    /// </summary>
    public class JSONMessage // (jsonstream.Message)
    {
        [JsonPropertyName("stream")]
        public string? Stream { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("progressDetail")]
        public JSONProgress? Progress { get; set; }

        [JsonPropertyName("id")]
        public string? ID { get; set; }

        [JsonPropertyName("errorDetail")]
        public JSONError? Error { get; set; }

        [JsonPropertyName("aux")]
        public ObjectExtensionData? Aux { get; set; }
    }
}

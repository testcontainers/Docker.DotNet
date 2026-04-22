#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Message defines a message struct. It describes
    /// the created time, where it from, status, ID of the
    /// message.
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

        /// <summary>
        /// Aux contains out-of-band data, such as digests for push signing and image id after building.
        /// </summary>
        [JsonPropertyName("aux")]
        public ObjectExtensionData? Aux { get; set; }
    }
}

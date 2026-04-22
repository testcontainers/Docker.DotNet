#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Error wraps a concrete Code and Message, Code is
    /// an integer error code, Message is the error message.
    /// </summary>
    public class JSONError // (jsonstream.Error)
    {
        [JsonPropertyName("code")]
        public long? Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}

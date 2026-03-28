#nullable enable
namespace Docker.DotNet.Models
{
    public class Message // (events.Message)
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("Action")]
        public string Action { get; set; } = default!;

        [JsonPropertyName("Actor")]
        public Actor Actor { get; set; } = default!;

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("time")]
        public long? Time { get; set; }

        [JsonPropertyName("timeNano")]
        public long? TimeNano { get; set; }
    }
}

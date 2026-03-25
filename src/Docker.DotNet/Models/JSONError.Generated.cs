#nullable enable
namespace Docker.DotNet.Models
{
    public class JSONError // (jsonstream.Error)
    {
        [JsonPropertyName("code")]
        public long Code { get; set; } = default!;

        [JsonPropertyName("message")]
        public string Message { get; set; } = default!;
    }
}

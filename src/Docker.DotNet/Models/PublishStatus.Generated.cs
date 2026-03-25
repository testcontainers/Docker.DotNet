#nullable enable
namespace Docker.DotNet.Models
{
    public class PublishStatus // (volume.PublishStatus)
    {
        [JsonPropertyName("NodeID")]
        public string NodeID { get; set; } = default!;

        [JsonPropertyName("State")]
        public string State { get; set; } = default!;

        [JsonPropertyName("PublishContext")]
        public IDictionary<string, string> PublishContext { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class PullIdentity // (image.PullIdentity)
    {
        [JsonPropertyName("Repository")]
        public string? Repository { get; set; }
    }
}

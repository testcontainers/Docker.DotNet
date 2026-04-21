#nullable enable
namespace Docker.DotNet.Models
{
    public class BuildIdentity // (image.BuildIdentity)
    {
        [JsonPropertyName("Ref")]
        public string? Ref { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime? CreatedAt { get; set; }
    }
}

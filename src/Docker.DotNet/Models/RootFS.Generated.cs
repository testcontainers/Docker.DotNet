#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RootFS returns Image&apos;s RootFS description including the layer IDs.
    /// </summary>
    public class RootFS // (image.RootFS)
    {
        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Layers")]
        public IList<string>? Layers { get; set; }
    }
}

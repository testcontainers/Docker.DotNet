#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ImageBuildResult holds information
    /// returned by a server after building
    /// an image.
    /// </summary>
    public class ImageBuildResult // (client.ImageBuildResult)
    {
        [JsonPropertyName("Body")]
        public object Body { get; set; } = default!;
    }
}

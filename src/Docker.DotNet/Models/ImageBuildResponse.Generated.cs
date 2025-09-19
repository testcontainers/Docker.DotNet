namespace Docker.DotNet.Models
{
    public class ImageBuildResponse // (build.ImageBuildResponse)
    {
        [JsonPropertyName("Body")]
        public object Body { get; set; }

        [JsonPropertyName("OSType")]
        public string OSType { get; set; }
    }
}

namespace Docker.DotNet.Models
{
    public class ImagesLoadResponse // (image.LoadResponse)
    {
        [JsonPropertyName("Body")]
        public object Body { get; set; }

        [JsonPropertyName("JSON")]
        public bool JSON { get; set; }
    }
}

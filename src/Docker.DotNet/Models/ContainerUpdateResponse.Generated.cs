namespace Docker.DotNet.Models
{
    public class ContainerUpdateResponse // (main.ContainerUpdateResponse)
    {
        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; }
    }
}

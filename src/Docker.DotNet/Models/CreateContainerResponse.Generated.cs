namespace Docker.DotNet.Models
{
    public class CreateContainerResponse // (container.CreateResponse)
    {
        [JsonPropertyName("Id")]
        public string ID { get; set; }

        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; }
    }
}

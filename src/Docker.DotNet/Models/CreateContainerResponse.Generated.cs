#nullable enable
namespace Docker.DotNet.Models
{
    public class CreateContainerResponse // (container.CreateResponse)
    {
        [JsonPropertyName("Id")]
        public string ID { get; set; } = default!;

        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; } = default!;
    }
}

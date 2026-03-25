#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerProcessesResponse // (container.TopResponse)
    {
        [JsonPropertyName("Processes")]
        public IList<IList<string>> Processes { get; set; } = default!;

        [JsonPropertyName("Titles")]
        public IList<string> Titles { get; set; } = default!;
    }
}

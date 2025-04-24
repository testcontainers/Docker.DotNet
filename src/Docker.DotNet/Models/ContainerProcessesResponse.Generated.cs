using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class ContainerProcessesResponse // (container.TopResponse)
    {
        [JsonPropertyName("Processes")]
        public IList<IList<string>> Processes { get; set; }

        [JsonPropertyName("Titles")]
        public IList<string> Titles { get; set; }
    }
}

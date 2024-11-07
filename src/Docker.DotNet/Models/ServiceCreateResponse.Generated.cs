using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class ServiceCreateResponse // (swarm.ServiceCreateResponse)
    {
        [JsonPropertyName("ID")]
        public string ID { get; set; }

        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; }
    }
}

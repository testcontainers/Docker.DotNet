namespace Docker.DotNet.Models
{
    public class ServiceUpdateResponse // (swarm.ServiceUpdateResponse)
    {
        [JsonPropertyName("Warnings")]
        public IList<string> Warnings { get; set; }
    }
}

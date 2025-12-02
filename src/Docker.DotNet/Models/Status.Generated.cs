namespace Docker.DotNet.Models
{
    public class Status // (network.Status)
    {
        [JsonPropertyName("IPAM")]
        public IPAMStatus IPAM { get; set; }
    }
}

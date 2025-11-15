namespace Docker.DotNet.Models
{
    public class IPAMStatus // (network.IPAMStatus)
    {
        [JsonPropertyName("Subnets")]
        public IDictionary<string, SubnetStatus> Subnets { get; set; }
    }
}

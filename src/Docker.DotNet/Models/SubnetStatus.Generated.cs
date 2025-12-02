namespace Docker.DotNet.Models
{
    public class SubnetStatus // (network.SubnetStatus)
    {
        [JsonPropertyName("IPsInUse")]
        public ulong IPsInUse { get; set; }

        [JsonPropertyName("DynamicIPsAvailable")]
        public ulong DynamicIPsAvailable { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// IPAMStatus IPAM status
    /// 
    /// swagger:model IPAMStatus
    /// </summary>
    public class IPAMStatus // (network.IPAMStatus)
    {
        /// <summary>
        /// subnets
        /// Example: {&quot;172.16.0.0/16&quot;:{&quot;DynamicIPsAvailable&quot;:65533,&quot;IPsInUse&quot;:3},&quot;2001:db8:abcd:0012::0/96&quot;:{&quot;DynamicIPsAvailable&quot;:4294967291,&quot;IPsInUse&quot;:5}}
        /// </summary>
        [JsonPropertyName("Subnets")]
        public IDictionary<string, SubnetStatus>? Subnets { get; set; }
    }
}

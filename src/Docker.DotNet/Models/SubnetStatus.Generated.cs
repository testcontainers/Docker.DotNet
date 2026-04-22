#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// SubnetStatus subnet status
    /// 
    /// swagger:model SubnetStatus
    /// </summary>
    public class SubnetStatus // (network.SubnetStatus)
    {
        /// <summary>
        /// Number of IP addresses in the subnet that are in use or reserved and are therefore unavailable for allocation, saturating at 2&lt;sup&gt;64&lt;/sup&gt; - 1.
        /// </summary>
        [JsonPropertyName("IPsInUse")]
        public ulong IPsInUse { get; set; } = default!;

        /// <summary>
        /// Number of IP addresses within the network&apos;s IPRange for the subnet that are available for allocation, saturating at 2&lt;sup&gt;64&lt;/sup&gt; - 1.
        /// </summary>
        [JsonPropertyName("DynamicIPsAvailable")]
        public ulong DynamicIPsAvailable { get; set; } = default!;
    }
}

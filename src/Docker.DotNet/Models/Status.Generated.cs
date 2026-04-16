#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Status provides runtime information about the network such as the number of allocated IPs.
    /// 
    /// swagger:model Status
    /// </summary>
    public class Status // (network.Status)
    {
        /// <summary>
        /// IPAM
        /// </summary>
        [JsonPropertyName("IPAM")]
        public IPAMStatus IPAM { get; set; } = default!;
    }
}

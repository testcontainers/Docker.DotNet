#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ServiceInfo represents service parameters with the list of service&apos;s tasks
    /// 
    /// swagger:model ServiceInfo
    /// </summary>
    public class ServiceInfo // (network.ServiceInfo)
    {
        /// <summary>
        /// v IP
        /// </summary>
        [JsonPropertyName("VIP")]
        public string VIP { get; set; } = default!;

        /// <summary>
        /// ports
        /// </summary>
        [JsonPropertyName("Ports")]
        public IList<string> Ports { get; set; } = default!;

        /// <summary>
        /// local l b index
        /// </summary>
        [JsonPropertyName("LocalLBIndex")]
        public long LocalLBIndex { get; set; } = default!;

        /// <summary>
        /// tasks
        /// </summary>
        [JsonPropertyName("Tasks")]
        public IList<NetworkTask> Tasks { get; set; } = default!;
    }
}

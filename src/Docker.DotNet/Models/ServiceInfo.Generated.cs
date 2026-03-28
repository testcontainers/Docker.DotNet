#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceInfo // (network.ServiceInfo)
    {
        [JsonPropertyName("VIP")]
        public string VIP { get; set; } = default!;

        [JsonPropertyName("Ports")]
        public IList<string> Ports { get; set; } = default!;

        [JsonPropertyName("LocalLBIndex")]
        public long LocalLBIndex { get; set; } = default!;

        [JsonPropertyName("Tasks")]
        public IList<NetworkTask> Tasks { get; set; } = default!;
    }
}

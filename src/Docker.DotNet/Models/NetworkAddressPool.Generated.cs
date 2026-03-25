#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworkAddressPool // (system.NetworkAddressPool)
    {
        [JsonPropertyName("Base")]
        public string Base { get; set; } = default!;

        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;
    }
}

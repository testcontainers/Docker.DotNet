#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkAddressPool is a temp struct used by [Info] struct.
    /// </summary>
    public class NetworkAddressPool // (system.NetworkAddressPool)
    {
        [JsonPropertyName("Base")]
        public string Base { get; set; } = default!;

        [JsonPropertyName("Size")]
        public long Size { get; set; } = default!;
    }
}

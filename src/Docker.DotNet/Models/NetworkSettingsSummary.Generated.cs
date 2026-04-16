#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// NetworkSettingsSummary provides a summary of container&apos;s networks
    /// in /containers/json
    /// </summary>
    public class NetworkSettingsSummary // (container.NetworkSettingsSummary)
    {
        [JsonPropertyName("Networks")]
        public IDictionary<string, EndpointSettings> Networks { get; set; } = default!;
    }
}

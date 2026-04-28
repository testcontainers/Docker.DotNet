#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DistributionInspectResponse contains response of Engine API:
    /// GET &quot;/distribution/{name}/json&quot;
    /// </summary>
    public class DistributionInspectResponse // (registry.DistributionInspect)
    {
        [JsonPropertyName("Descriptor")]
        public Descriptor Descriptor { get; set; } = default!;

        [JsonPropertyName("Platforms")]
        public IList<Platform> Platforms { get; set; } = default!;
    }
}

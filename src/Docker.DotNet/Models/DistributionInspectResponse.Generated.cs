#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// DistributionInspect describes the result obtained from contacting the
    /// registry to retrieve image metadata
    /// </summary>
    public class DistributionInspectResponse // (registry.DistributionInspect)
    {
        /// <summary>
        /// Descriptor contains information about the manifest, including
        /// the content addressable digest
        /// </summary>
        [JsonPropertyName("Descriptor")]
        public Descriptor Descriptor { get; set; } = default!;

        /// <summary>
        /// Platforms contains the list of platforms supported by the image,
        /// obtained by parsing the manifest
        /// </summary>
        [JsonPropertyName("Platforms")]
        public IList<Platform> Platforms { get; set; } = default!;
    }
}

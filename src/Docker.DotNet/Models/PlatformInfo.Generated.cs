#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PlatformInfo holds information about the platform (product name) the
    /// server is running on.
    /// </summary>
    public class PlatformInfo // (system.PlatformInfo)
    {
        /// <summary>
        /// Name is the name of the platform (for example, &quot;Docker Engine - Community&quot;,
        /// or &quot;Docker Desktop 4.49.0 (208003)&quot;)
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = default!;
    }
}

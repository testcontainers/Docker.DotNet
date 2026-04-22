#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// VersionResponse contains information about the Docker server host.
    /// GET &quot;/version&quot;
    /// </summary>
    public class VersionResponse // (system.VersionResponse)
    {
        /// <summary>
        /// Platform is the platform (product name) the server is running on.
        /// </summary>
        [JsonPropertyName("Platform")]
        public PlatformInfo? Platform { get; set; }

        /// <summary>
        /// Version is the version of the daemon.
        /// </summary>
        [JsonPropertyName("Version")]
        public string Version { get; set; } = default!;

        /// <summary>
        /// APIVersion is the highest API version supported by the server.
        /// </summary>
        [JsonPropertyName("ApiVersion")]
        public string APIVersion { get; set; } = default!;

        /// <summary>
        /// MinAPIVersion is the minimum API version the server supports.
        /// </summary>
        [JsonPropertyName("MinAPIVersion")]
        public string? MinAPIVersion { get; set; }

        /// <summary>
        /// Os is the operating system the server runs on.
        /// </summary>
        [JsonPropertyName("Os")]
        public string Os { get; set; } = default!;

        /// <summary>
        /// Arch is the hardware architecture the server runs on.
        /// </summary>
        [JsonPropertyName("Arch")]
        public string Arch { get; set; } = default!;

        /// <summary>
        /// Components contains version information for the components making
        /// up the server. Information in this field is for informational
        /// purposes, and not part of the API contract.
        /// </summary>
        [JsonPropertyName("Components")]
        public IList<ComponentVersion>? Components { get; set; }

        [JsonPropertyName("GitCommit")]
        public string? GitCommit { get; set; }

        [JsonPropertyName("GoVersion")]
        public string? GoVersion { get; set; }

        [JsonPropertyName("KernelVersion")]
        public string? KernelVersion { get; set; }

        [JsonPropertyName("Experimental")]
        public bool? Experimental { get; set; }

        [JsonPropertyName("BuildTime")]
        public string? BuildTime { get; set; }
    }
}

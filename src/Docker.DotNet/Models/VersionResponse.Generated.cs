#nullable enable
namespace Docker.DotNet.Models
{
    public class VersionResponse // (system.VersionResponse)
    {
        [JsonPropertyName("Platform")]
        public PlatformInfo Platform { get; set; } = default!;

        [JsonPropertyName("Version")]
        public string Version { get; set; } = default!;

        [JsonPropertyName("ApiVersion")]
        public string APIVersion { get; set; } = default!;

        [JsonPropertyName("MinAPIVersion")]
        public string MinAPIVersion { get; set; } = default!;

        [JsonPropertyName("Os")]
        public string Os { get; set; } = default!;

        [JsonPropertyName("Arch")]
        public string Arch { get; set; } = default!;

        [JsonPropertyName("Components")]
        public IList<ComponentVersion> Components { get; set; } = default!;

        [JsonPropertyName("GitCommit")]
        public string GitCommit { get; set; } = default!;

        [JsonPropertyName("GoVersion")]
        public string GoVersion { get; set; } = default!;

        [JsonPropertyName("KernelVersion")]
        public string KernelVersion { get; set; } = default!;

        [JsonPropertyName("Experimental")]
        public bool Experimental { get; set; } = default!;

        [JsonPropertyName("BuildTime")]
        public string BuildTime { get; set; } = default!;
    }
}

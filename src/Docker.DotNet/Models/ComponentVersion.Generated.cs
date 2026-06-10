#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ComponentVersion describes the version information for a specific component.
    /// </summary>
    public class ComponentVersion // (system.ComponentVersion)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Version")]
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// Details contains Key/value pairs of strings with additional information
        /// about the component. These values are intended for informational purposes
        /// only, and their content is not defined, and not part of the API
        /// specification.
        /// 
        /// These messages can be printed by the client as information to the user.
        /// </summary>
        [JsonPropertyName("Details")]
        public IDictionary<string, string>? Details { get; set; }
    }
}

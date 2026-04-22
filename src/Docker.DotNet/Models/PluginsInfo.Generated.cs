#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PluginsInfo is a temp struct holding Plugins name
    /// registered with docker daemon. It is used by [Info] struct
    /// </summary>
    public class PluginsInfo // (system.PluginsInfo)
    {
        /// <summary>
        /// List of Volume plugins registered
        /// </summary>
        [JsonPropertyName("Volume")]
        public IList<string> Volume { get; set; } = default!;

        /// <summary>
        /// List of Network plugins registered
        /// </summary>
        [JsonPropertyName("Network")]
        public IList<string> Network { get; set; } = default!;

        /// <summary>
        /// List of Authorization plugins registered
        /// </summary>
        [JsonPropertyName("Authorization")]
        public IList<string> Authorization { get; set; } = default!;

        /// <summary>
        /// List of Log plugins registered
        /// </summary>
        [JsonPropertyName("Log")]
        public IList<string> Log { get; set; } = default!;
    }
}

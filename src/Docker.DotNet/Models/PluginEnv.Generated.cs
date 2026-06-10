#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Env env
    /// 
    /// swagger:model Env
    /// </summary>
    public class PluginEnv // (plugin.Env)
    {
        /// <summary>
        /// description
        /// Required: true
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// name
        /// Required: true
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// settable
        /// Required: true
        /// </summary>
        [JsonPropertyName("Settable")]
        public IList<string> Settable { get; set; } = default!;

        /// <summary>
        /// value
        /// Required: true
        /// </summary>
        [JsonPropertyName("Value")]
        public string? Value { get; set; }
    }
}

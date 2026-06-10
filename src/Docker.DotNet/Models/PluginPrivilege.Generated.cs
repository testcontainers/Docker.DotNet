#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Privilege describes a permission the user has to accept
    /// upon installing a plugin.
    /// </summary>
    public class PluginPrivilege // (plugin.Privilege)
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("Value")]
        public IList<string> Value { get; set; } = default!;
    }
}

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
        public string Name { get; set; } = default!;

        [JsonPropertyName("Description")]
        public string Description { get; set; } = default!;

        [JsonPropertyName("Value")]
        public IList<string> Value { get; set; } = default!;
    }
}

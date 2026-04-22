#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// RuntimePrivilege describes a permission the user has to accept
    /// upon installing a plugin.
    /// </summary>
    public class RuntimePrivilege // (swarm.RuntimePrivilege)
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("value")]
        public IList<string>? Value { get; set; }
    }
}

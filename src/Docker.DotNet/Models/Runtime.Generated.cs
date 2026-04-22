#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Runtime describes an OCI runtime
    /// </summary>
    public class Runtime // (system.Runtime)
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("runtimeArgs")]
        public IList<string>? Args { get; set; }

        [JsonPropertyName("runtimeType")]
        public string? Type { get; set; }

        [JsonPropertyName("options")]
        public IDictionary<string, object>? Options { get; set; }
    }
}

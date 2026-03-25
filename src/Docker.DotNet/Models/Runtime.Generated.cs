#nullable enable
namespace Docker.DotNet.Models
{
    public class Runtime // (system.Runtime)
    {
        [JsonPropertyName("path")]
        public string Path { get; set; } = default!;

        [JsonPropertyName("runtimeArgs")]
        public IList<string> Args { get; set; } = default!;

        [JsonPropertyName("runtimeType")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("options")]
        public IDictionary<string, object> Options { get; set; } = default!;
    }
}

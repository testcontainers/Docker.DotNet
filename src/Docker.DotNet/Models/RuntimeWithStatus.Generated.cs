#nullable enable
namespace Docker.DotNet.Models
{
    public class RuntimeWithStatus // (system.RuntimeWithStatus)
    {
        public RuntimeWithStatus()
        {
        }

        public RuntimeWithStatus(Runtime Runtime)
        {
            if (Runtime != null)
            {
                this.Path = Runtime.Path;
                this.Args = Runtime.Args;
                this.Type = Runtime.Type;
                this.Options = Runtime.Options;
            }
        }

        [JsonPropertyName("path")]
        public string Path { get; set; } = default!;

        [JsonPropertyName("runtimeArgs")]
        public IList<string> Args { get; set; } = default!;

        [JsonPropertyName("runtimeType")]
        public string Type { get; set; } = default!;

        [JsonPropertyName("options")]
        public IDictionary<string, object> Options { get; set; } = default!;

        [JsonPropertyName("status")]
        public IDictionary<string, string> Status { get; set; } = default!;
    }
}

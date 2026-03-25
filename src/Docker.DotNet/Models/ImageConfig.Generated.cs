#nullable enable
namespace Docker.DotNet.Models
{
    public class ImageConfig // (v1.ImageConfig)
    {
        [JsonPropertyName("User")]
        public string User { get; set; } = default!;

        [JsonPropertyName("ExposedPorts")]
        public IDictionary<string, EmptyStruct> ExposedPorts { get; set; } = default!;

        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; } = default!;

        [JsonPropertyName("Entrypoint")]
        public IList<string> Entrypoint { get; set; } = default!;

        [JsonPropertyName("Cmd")]
        public IList<string> Cmd { get; set; } = default!;

        [JsonPropertyName("Volumes")]
        public IDictionary<string, EmptyStruct> Volumes { get; set; } = default!;

        [JsonPropertyName("WorkingDir")]
        public string WorkingDir { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("StopSignal")]
        public string StopSignal { get; set; } = default!;

        [JsonPropertyName("ArgsEscaped")]
        public bool ArgsEscaped { get; set; } = default!;
    }
}

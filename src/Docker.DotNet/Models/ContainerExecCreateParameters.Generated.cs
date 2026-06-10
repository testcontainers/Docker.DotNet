#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerExecCreateParameters // (main.ContainerExecCreateParameters)
    {
        [JsonPropertyName("User")]
        public string User { get; set; } = string.Empty;

        [JsonPropertyName("Privileged")]
        public bool Privileged { get; set; } = default!;

        [JsonPropertyName("TTY")]
        public bool TTY { get; set; } = default!;

        [JsonPropertyName("ConsoleSize")]
        [JsonConverter(typeof(JsonConsoleSizeConverter))]
        public ConsoleSize ConsoleSize { get; set; } = default!;

        [JsonPropertyName("AttachStdin")]
        public bool AttachStdin { get; set; } = default!;

        [JsonPropertyName("AttachStderr")]
        public bool AttachStderr { get; set; } = default!;

        [JsonPropertyName("AttachStdout")]
        public bool AttachStdout { get; set; } = default!;

        [JsonPropertyName("DetachKeys")]
        public string DetachKeys { get; set; } = string.Empty;

        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; } = default!;

        [JsonPropertyName("WorkingDir")]
        public string WorkingDir { get; set; } = string.Empty;

        [JsonPropertyName("Cmd")]
        public IList<string> Cmd { get; set; } = default!;
    }
}

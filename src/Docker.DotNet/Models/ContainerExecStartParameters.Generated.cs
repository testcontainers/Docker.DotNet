#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerExecStartParameters // (main.ContainerExecStartParameters)
    {
        [JsonPropertyName("Detach")]
        public bool Detach { get; set; } = default!;

        [JsonPropertyName("TTY")]
        public bool TTY { get; set; } = default!;

        [JsonPropertyName("ConsoleSize")]
        [JsonConverter(typeof(JsonConsoleSizeConverter))]
        public ConsoleSize ConsoleSize { get; set; } = default!;
    }
}

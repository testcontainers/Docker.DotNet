namespace Docker.DotNet.Models
{
    public class ContainerExecStartParameters // (main.ContainerExecStartParameters)
    {
        [JsonPropertyName("Detach")]
        public bool Detach { get; set; }

        [JsonPropertyName("TTY")]
        public bool TTY { get; set; }

        [JsonPropertyName("ConsoleSize")]
        public ConsoleSize ConsoleSize { get; set; }
    }
}

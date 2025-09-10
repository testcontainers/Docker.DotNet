namespace Docker.DotNet.Models
{
    public class ContainerExecStartParameters // (main.ContainerExecStartParameters)
    {
        [JsonPropertyName("Detach")]
        public bool Detach { get; set; }

        [JsonPropertyName("Tty")]
        public bool Tty { get; set; }

        [JsonPropertyName("ConsoleSize")]
        public ulong[] ConsoleSize { get; set; }
    }
}

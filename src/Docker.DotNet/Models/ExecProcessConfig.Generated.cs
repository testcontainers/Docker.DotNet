#nullable enable
namespace Docker.DotNet.Models
{
    public class ExecProcessConfig // (container.ExecProcessConfig)
    {
        [JsonPropertyName("tty")]
        public bool Tty { get; set; } = default!;

        [JsonPropertyName("entrypoint")]
        public string Entrypoint { get; set; } = default!;

        [JsonPropertyName("arguments")]
        public IList<string> Arguments { get; set; } = default!;

        [JsonPropertyName("privileged")]
        public bool? Privileged { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; } = default!;
    }
}

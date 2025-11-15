namespace Docker.DotNet.Models
{
    public class ExecProcessConfig // (container.ExecProcessConfig)
    {
        [JsonPropertyName("tty")]
        public bool Tty { get; set; }

        [JsonPropertyName("entrypoint")]
        public string Entrypoint { get; set; }

        [JsonPropertyName("arguments")]
        public IList<string> Arguments { get; set; }

        [JsonPropertyName("privileged")]
        public bool? Privileged { get; set; }

        [JsonPropertyName("user")]
        public string User { get; set; }
    }
}

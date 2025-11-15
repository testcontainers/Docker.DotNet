namespace Docker.DotNet.Models
{
    public class ImageConfig // (v1.ImageConfig)
    {
        [JsonPropertyName("User")]
        public string User { get; set; }

        [JsonPropertyName("ExposedPorts")]
        public IDictionary<string, EmptyStruct> ExposedPorts { get; set; }

        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; }

        [JsonPropertyName("Entrypoint")]
        public IList<string> Entrypoint { get; set; }

        [JsonPropertyName("Cmd")]
        public IList<string> Cmd { get; set; }

        [JsonPropertyName("Volumes")]
        public IDictionary<string, EmptyStruct> Volumes { get; set; }

        [JsonPropertyName("WorkingDir")]
        public string WorkingDir { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; }

        [JsonPropertyName("StopSignal")]
        public string StopSignal { get; set; }

        [JsonPropertyName("ArgsEscaped")]
        public bool ArgsEscaped { get; set; }
    }
}

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
        public string Path { get; set; }

        [JsonPropertyName("runtimeArgs")]
        public IList<string> Args { get; set; }

        [JsonPropertyName("runtimeType")]
        public string Type { get; set; }

        [JsonPropertyName("options")]
        public IDictionary<string, object> Options { get; set; }

        [JsonPropertyName("status")]
        public IDictionary<string, string> Status { get; set; }
    }
}

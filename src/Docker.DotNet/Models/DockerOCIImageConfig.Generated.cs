#nullable enable
namespace Docker.DotNet.Models
{
    public class DockerOCIImageConfig // (v1.DockerOCIImageConfig)
    {
        public DockerOCIImageConfig()
        {
        }

        public DockerOCIImageConfig(ImageConfig ImageConfig, DockerOCIImageConfigExt DockerOCIImageConfigExt)
        {
            if (ImageConfig != null)
            {
                this.User = ImageConfig.User;
                this.ExposedPorts = ImageConfig.ExposedPorts;
                this.Env = ImageConfig.Env;
                this.Entrypoint = ImageConfig.Entrypoint;
                this.Cmd = ImageConfig.Cmd;
                this.Volumes = ImageConfig.Volumes;
                this.WorkingDir = ImageConfig.WorkingDir;
                this.Labels = ImageConfig.Labels;
                this.StopSignal = ImageConfig.StopSignal;
                this.ArgsEscaped = ImageConfig.ArgsEscaped;
            }

            if (DockerOCIImageConfigExt != null)
            {
                this.Healthcheck = DockerOCIImageConfigExt.Healthcheck;
                this.OnBuild = DockerOCIImageConfigExt.OnBuild;
                this.Shell = DockerOCIImageConfigExt.Shell;
            }
        }

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

        [JsonPropertyName("Healthcheck")]
        public HealthcheckConfig? Healthcheck { get; set; }

        [JsonPropertyName("OnBuild")]
        public IList<string> OnBuild { get; set; } = default!;

        [JsonPropertyName("Shell")]
        public IList<string> Shell { get; set; } = default!;
    }
}

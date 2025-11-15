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

        [JsonPropertyName("Healthcheck")]
        public HealthcheckConfig Healthcheck { get; set; }

        [JsonPropertyName("OnBuild")]
        public IList<string> OnBuild { get; set; }

        [JsonPropertyName("Shell")]
        public IList<string> Shell { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class CommitContainerChangesParameters // (main.CommitContainerChangesParameters)
    {
        public CommitContainerChangesParameters()
        {
        }

        public CommitContainerChangesParameters(ContainerConfig Config)
        {
            if (Config != null)
            {
                this.Hostname = Config.Hostname;
                this.Domainname = Config.Domainname;
                this.User = Config.User;
                this.AttachStdin = Config.AttachStdin;
                this.AttachStdout = Config.AttachStdout;
                this.AttachStderr = Config.AttachStderr;
                this.ExposedPorts = Config.ExposedPorts;
                this.Tty = Config.Tty;
                this.OpenStdin = Config.OpenStdin;
                this.StdinOnce = Config.StdinOnce;
                this.Env = Config.Env;
                this.Cmd = Config.Cmd;
                this.Healthcheck = Config.Healthcheck;
                this.ArgsEscaped = Config.ArgsEscaped;
                this.Image = Config.Image;
                this.Volumes = Config.Volumes;
                this.WorkingDir = Config.WorkingDir;
                this.Entrypoint = Config.Entrypoint;
                this.NetworkDisabled = Config.NetworkDisabled;
                this.OnBuild = Config.OnBuild;
                this.Labels = Config.Labels;
                this.StopSignal = Config.StopSignal;
                this.StopTimeout = Config.StopTimeout;
                this.Shell = Config.Shell;
            }
        }

        [QueryStringParameter("container", true)]
        public string ContainerID { get; set; } = default!;

        [QueryStringParameter("repo", false)]
        public string? RepositoryName { get; set; }

        [QueryStringParameter("tag", false)]
        public string? Tag { get; set; }

        [QueryStringParameter("comment", false)]
        public string? Comment { get; set; }

        [QueryStringParameter("author", false)]
        public string? Author { get; set; }

        [QueryStringParameter<EnumerableQueryStringConverter>("changes", false)]
        public IList<string>? Changes { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("pause", false)]
        public bool? Pause { get; set; }

        [JsonPropertyName("Hostname")]
        public string Hostname { get; set; } = default!;

        [JsonPropertyName("Domainname")]
        public string Domainname { get; set; } = default!;

        [JsonPropertyName("User")]
        public string User { get; set; } = default!;

        [JsonPropertyName("AttachStdin")]
        public bool AttachStdin { get; set; } = default!;

        [JsonPropertyName("AttachStdout")]
        public bool AttachStdout { get; set; } = default!;

        [JsonPropertyName("AttachStderr")]
        public bool AttachStderr { get; set; } = default!;

        [JsonPropertyName("ExposedPorts")]
        public IDictionary<string, EmptyStruct>? ExposedPorts { get; set; }

        [JsonPropertyName("Tty")]
        public bool Tty { get; set; } = default!;

        [JsonPropertyName("OpenStdin")]
        public bool OpenStdin { get; set; } = default!;

        [JsonPropertyName("StdinOnce")]
        public bool StdinOnce { get; set; } = default!;

        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; } = default!;

        [JsonPropertyName("Cmd")]
        public IList<string> Cmd { get; set; } = default!;

        [JsonPropertyName("Healthcheck")]
        public HealthcheckConfig? Healthcheck { get; set; }

        [JsonPropertyName("ArgsEscaped")]
        public bool? ArgsEscaped { get; set; }

        [JsonPropertyName("Image")]
        public string Image { get; set; } = default!;

        [JsonPropertyName("Volumes")]
        public IDictionary<string, EmptyStruct> Volumes { get; set; } = default!;

        [JsonPropertyName("WorkingDir")]
        public string WorkingDir { get; set; } = default!;

        [JsonPropertyName("Entrypoint")]
        public IList<string> Entrypoint { get; set; } = default!;

        [JsonPropertyName("NetworkDisabled")]
        public bool? NetworkDisabled { get; set; }

        [JsonPropertyName("OnBuild")]
        public IList<string>? OnBuild { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("StopSignal")]
        public string? StopSignal { get; set; }

        [JsonPropertyName("StopTimeout")]
        [JsonConverter(typeof(TimeSpanSecondsConverter))]
        public TimeSpan? StopTimeout { get; set; }

        [JsonPropertyName("Shell")]
        public IList<string>? Shell { get; set; }
    }
}

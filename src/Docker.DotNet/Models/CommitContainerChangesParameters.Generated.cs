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

        [QueryStringListParameter("changes", false)]
        public IList<string>? Changes { get; set; }

        [QueryStringBoolParameter("pause", false)]
        public bool? Pause { get; set; }

        /// <summary>
        /// Hostname
        /// </summary>
        [JsonPropertyName("Hostname")]
        public string Hostname { get; set; } = default!;

        /// <summary>
        /// Domainname
        /// </summary>
        [JsonPropertyName("Domainname")]
        public string Domainname { get; set; } = default!;

        /// <summary>
        /// User that will run the command(s) inside the container, also support user:group
        /// </summary>
        [JsonPropertyName("User")]
        public string User { get; set; } = default!;

        /// <summary>
        /// Attach the standard input, makes possible user interaction
        /// </summary>
        [JsonPropertyName("AttachStdin")]
        public bool AttachStdin { get; set; } = default!;

        /// <summary>
        /// Attach the standard output
        /// </summary>
        [JsonPropertyName("AttachStdout")]
        public bool AttachStdout { get; set; } = default!;

        /// <summary>
        /// Attach the standard error
        /// </summary>
        [JsonPropertyName("AttachStderr")]
        public bool AttachStderr { get; set; } = default!;

        /// <summary>
        /// List of exposed ports
        /// </summary>
        [JsonPropertyName("ExposedPorts")]
        public IDictionary<string, EmptyStruct>? ExposedPorts { get; set; }

        /// <summary>
        /// Attach standard streams to a tty, including stdin if it is not closed.
        /// </summary>
        [JsonPropertyName("Tty")]
        public bool Tty { get; set; } = default!;

        /// <summary>
        /// Open stdin
        /// </summary>
        [JsonPropertyName("OpenStdin")]
        public bool OpenStdin { get; set; } = default!;

        /// <summary>
        /// If true, close stdin after the 1 attached client disconnects.
        /// </summary>
        [JsonPropertyName("StdinOnce")]
        public bool StdinOnce { get; set; } = default!;

        /// <summary>
        /// List of environment variable to set in the container
        /// </summary>
        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; } = default!;

        /// <summary>
        /// Command to run when starting the container
        /// </summary>
        [JsonPropertyName("Cmd")]
        public IList<string> Cmd { get; set; } = default!;

        /// <summary>
        /// Healthcheck describes how to check the container is healthy
        /// </summary>
        [JsonPropertyName("Healthcheck")]
        public HealthcheckConfig? Healthcheck { get; set; }

        /// <summary>
        /// True if command is already escaped (meaning treat as a command line) (Windows specific).
        /// </summary>
        [JsonPropertyName("ArgsEscaped")]
        public bool? ArgsEscaped { get; set; }

        /// <summary>
        /// Name of the image as it was passed by the operator (e.g. could be symbolic)
        /// </summary>
        [JsonPropertyName("Image")]
        public string Image { get; set; } = default!;

        /// <summary>
        /// List of volumes (mounts) used for the container
        /// </summary>
        [JsonPropertyName("Volumes")]
        public IDictionary<string, EmptyStruct> Volumes { get; set; } = default!;

        /// <summary>
        /// Current directory (PWD) in the command will be launched
        /// </summary>
        [JsonPropertyName("WorkingDir")]
        public string WorkingDir { get; set; } = default!;

        /// <summary>
        /// Entrypoint to run when starting the container
        /// </summary>
        [JsonPropertyName("Entrypoint")]
        public IList<string> Entrypoint { get; set; } = default!;

        /// <summary>
        /// Is network disabled
        /// </summary>
        [JsonPropertyName("NetworkDisabled")]
        public bool? NetworkDisabled { get; set; }

        /// <summary>
        /// ONBUILD metadata that were defined on the image Dockerfile
        /// </summary>
        [JsonPropertyName("OnBuild")]
        public IList<string>? OnBuild { get; set; }

        /// <summary>
        /// List of labels set to this container
        /// </summary>
        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        /// <summary>
        /// Signal to stop a container
        /// </summary>
        [JsonPropertyName("StopSignal")]
        public string? StopSignal { get; set; }

        /// <summary>
        /// Timeout (in seconds) to stop a container
        /// </summary>
        [JsonPropertyName("StopTimeout")]
        [JsonConverter(typeof(JsonTimeSpanSecondsConverter))]
        public TimeSpan? StopTimeout { get; set; }

        /// <summary>
        /// Shell for shell-form of RUN, CMD, ENTRYPOINT
        /// </summary>
        [JsonPropertyName("Shell")]
        public IList<string>? Shell { get; set; }
    }
}

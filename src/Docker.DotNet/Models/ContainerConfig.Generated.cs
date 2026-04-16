#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// Config contains the configuration data about a container.
    /// It should hold only portable information about the container.
    /// Here, &quot;portable&quot; means &quot;independent from the host we are running on&quot;.
    /// Non-portable information *should* appear in HostConfig.
    /// All fields added to this struct must be marked `omitempty` to keep getting
    /// predictable hashes from the old `v1Compatibility` configuration.
    /// </summary>
    public class ContainerConfig // (container.Config)
    {
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

#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerSpec // (swarm.ContainerSpec)
    {
        [JsonPropertyName("Image")]
        public string Image { get; set; } = default!;

        [JsonPropertyName("Labels")]
        public IDictionary<string, string> Labels { get; set; } = default!;

        [JsonPropertyName("Command")]
        public IList<string> Command { get; set; } = default!;

        [JsonPropertyName("Args")]
        public IList<string> Args { get; set; } = default!;

        [JsonPropertyName("Hostname")]
        public string Hostname { get; set; } = default!;

        [JsonPropertyName("Env")]
        public IList<string> Env { get; set; } = default!;

        [JsonPropertyName("Dir")]
        public string Dir { get; set; } = default!;

        [JsonPropertyName("User")]
        public string User { get; set; } = default!;

        [JsonPropertyName("Groups")]
        public IList<string> Groups { get; set; } = default!;

        [JsonPropertyName("Privileges")]
        public Privileges? Privileges { get; set; }

        [JsonPropertyName("Init")]
        public bool? Init { get; set; }

        [JsonPropertyName("StopSignal")]
        public string StopSignal { get; set; } = default!;

        [JsonPropertyName("TTY")]
        public bool TTY { get; set; } = default!;

        [JsonPropertyName("OpenStdin")]
        public bool OpenStdin { get; set; } = default!;

        [JsonPropertyName("ReadOnly")]
        public bool ReadOnly { get; set; } = default!;

        [JsonPropertyName("Mounts")]
        public IList<Mount> Mounts { get; set; } = default!;

        [JsonPropertyName("StopGracePeriod")]
        public long? StopGracePeriod { get; set; }

        [JsonPropertyName("Healthcheck")]
        public HealthcheckConfig? Healthcheck { get; set; }

        [JsonPropertyName("Hosts")]
        public IList<string> Hosts { get; set; } = default!;

        [JsonPropertyName("DNSConfig")]
        public DNSConfig? DNSConfig { get; set; }

        [JsonPropertyName("Secrets")]
        public IList<SecretReference> Secrets { get; set; } = default!;

        [JsonPropertyName("Configs")]
        public IList<SwarmConfigReference> Configs { get; set; } = default!;

        [JsonPropertyName("Isolation")]
        public string Isolation { get; set; } = default!;

        [JsonPropertyName("Sysctls")]
        public IDictionary<string, string> Sysctls { get; set; } = default!;

        [JsonPropertyName("CapabilityAdd")]
        public IList<string> CapabilityAdd { get; set; } = default!;

        [JsonPropertyName("CapabilityDrop")]
        public IList<string> CapabilityDrop { get; set; } = default!;

        [JsonPropertyName("Ulimits")]
        public IList<Ulimit> Ulimits { get; set; } = default!;

        [JsonPropertyName("OomScoreAdj")]
        public long OomScoreAdj { get; set; } = default!;
    }
}

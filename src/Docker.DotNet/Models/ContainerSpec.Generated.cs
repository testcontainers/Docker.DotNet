#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// ContainerSpec represents the spec of a container.
    /// </summary>
    public class ContainerSpec // (swarm.ContainerSpec)
    {
        [JsonPropertyName("Image")]
        public string? Image { get; set; }

        [JsonPropertyName("Labels")]
        public IDictionary<string, string>? Labels { get; set; }

        [JsonPropertyName("Command")]
        public IList<string>? Command { get; set; }

        [JsonPropertyName("Args")]
        public IList<string>? Args { get; set; }

        [JsonPropertyName("Hostname")]
        public string? Hostname { get; set; }

        [JsonPropertyName("Env")]
        public IList<string>? Env { get; set; }

        [JsonPropertyName("Dir")]
        public string? Dir { get; set; }

        [JsonPropertyName("User")]
        public string? User { get; set; }

        [JsonPropertyName("Groups")]
        public IList<string>? Groups { get; set; }

        [JsonPropertyName("Privileges")]
        public Privileges? Privileges { get; set; }

        [JsonPropertyName("Init")]
        public bool? Init { get; set; }

        [JsonPropertyName("StopSignal")]
        public string? StopSignal { get; set; }

        [JsonPropertyName("TTY")]
        public bool? TTY { get; set; }

        [JsonPropertyName("OpenStdin")]
        public bool? OpenStdin { get; set; }

        [JsonPropertyName("ReadOnly")]
        public bool? ReadOnly { get; set; }

        [JsonPropertyName("Mounts")]
        public IList<Mount>? Mounts { get; set; }

        [JsonPropertyName("StopGracePeriod")]
        public long? StopGracePeriod { get; set; }

        [JsonPropertyName("Healthcheck")]
        public HealthcheckConfig? Healthcheck { get; set; }

        /// <summary>
        /// The format of extra hosts on swarmkit is specified in:
        /// http://man7.org/linux/man-pages/man5/hosts.5.html
        ///    IP_address canonical_hostname [aliases...]
        /// </summary>
        [JsonPropertyName("Hosts")]
        public IList<string>? Hosts { get; set; }

        [JsonPropertyName("DNSConfig")]
        public DNSConfig? DNSConfig { get; set; }

        [JsonPropertyName("Secrets")]
        public IList<SecretReference>? Secrets { get; set; }

        [JsonPropertyName("Configs")]
        public IList<SwarmConfigReference>? Configs { get; set; }

        [JsonPropertyName("Isolation")]
        public string? Isolation { get; set; }

        [JsonPropertyName("Sysctls")]
        public IDictionary<string, string>? Sysctls { get; set; }

        [JsonPropertyName("CapabilityAdd")]
        public IList<string>? CapabilityAdd { get; set; }

        [JsonPropertyName("CapabilityDrop")]
        public IList<string>? CapabilityDrop { get; set; }

        [JsonPropertyName("Ulimits")]
        public IList<Ulimit>? Ulimits { get; set; }

        [JsonPropertyName("OomScoreAdj")]
        public long? OomScoreAdj { get; set; }
    }
}

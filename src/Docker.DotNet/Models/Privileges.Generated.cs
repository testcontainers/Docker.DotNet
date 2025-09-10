namespace Docker.DotNet.Models
{
    public class Privileges // (swarm.Privileges)
    {
        [JsonPropertyName("CredentialSpec")]
        public CredentialSpec CredentialSpec { get; set; }

        [JsonPropertyName("SELinuxContext")]
        public SELinuxContext SELinuxContext { get; set; }

        [JsonPropertyName("Seccomp")]
        public SeccompOpts Seccomp { get; set; }

        [JsonPropertyName("AppArmor")]
        public AppArmorOpts AppArmor { get; set; }

        [JsonPropertyName("NoNewPrivileges")]
        public bool NoNewPrivileges { get; set; }
    }
}

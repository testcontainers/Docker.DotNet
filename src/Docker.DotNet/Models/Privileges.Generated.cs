using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class Privileges // (swarm.Privileges)
    {
        [DataMember(Name = "CredentialSpec", EmitDefaultValue = false)]
        public CredentialSpec CredentialSpec { get; set; }

        [DataMember(Name = "SELinuxContext", EmitDefaultValue = false)]
        public SELinuxContext SELinuxContext { get; set; }

        [DataMember(Name = "Seccomp", EmitDefaultValue = false)]
        public SeccompOpts Seccomp { get; set; }

        [DataMember(Name = "AppArmor", EmitDefaultValue = false)]
        public AppArmorOpts AppArmor { get; set; }

        [DataMember(Name = "NoNewPrivileges", EmitDefaultValue = false)]
        public bool NoNewPrivileges { get; set; }
    }
}

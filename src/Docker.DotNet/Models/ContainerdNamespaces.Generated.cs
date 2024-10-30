using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class ContainerdNamespaces // (system.ContainerdNamespaces)
    {
        [DataMember(Name = "Containers", EmitDefaultValue = false)]
        public string Containers { get; set; }

        [DataMember(Name = "Plugins", EmitDefaultValue = false)]
        public string Plugins { get; set; }
    }
}

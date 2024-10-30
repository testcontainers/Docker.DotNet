using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class ContainerdInfo // (system.ContainerdInfo)
    {
        [DataMember(Name = "Address", EmitDefaultValue = false)]
        public string Address { get; set; }

        [DataMember(Name = "Namespaces", EmitDefaultValue = false)]
        public ContainerdNamespaces Namespaces { get; set; }
    }
}

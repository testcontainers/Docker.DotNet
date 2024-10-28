using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class NetworkAddressPool // (system.NetworkAddressPool)
    {
        [DataMember(Name = "Base", EmitDefaultValue = false)]
        public string Base { get; set; }

        [DataMember(Name = "Size", EmitDefaultValue = false)]
        public long Size { get; set; }
    }
}

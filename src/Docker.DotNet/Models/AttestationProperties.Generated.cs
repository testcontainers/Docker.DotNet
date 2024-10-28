using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class AttestationProperties // (image.AttestationProperties)
    {
        [DataMember(Name = "For", EmitDefaultValue = false)]
        public string For { get; set; }
    }
}

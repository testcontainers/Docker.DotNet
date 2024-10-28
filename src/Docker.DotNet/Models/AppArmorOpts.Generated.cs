using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class AppArmorOpts // (swarm.AppArmorOpts)
    {
        [DataMember(Name = "Mode", EmitDefaultValue = false)]
        public string Mode { get; set; }
    }
}

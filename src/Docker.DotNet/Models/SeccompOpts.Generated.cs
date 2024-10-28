using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class SeccompOpts // (swarm.SeccompOpts)
    {
        [DataMember(Name = "Mode", EmitDefaultValue = false)]
        public string Mode { get; set; }

        [DataMember(Name = "Profile", EmitDefaultValue = false)]
        public IList<byte> Profile { get; set; }
    }
}

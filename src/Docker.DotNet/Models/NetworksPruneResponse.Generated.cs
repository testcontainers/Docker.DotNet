using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class NetworksPruneResponse // (network.PruneReport)
    {
        [DataMember(Name = "NetworksDeleted", EmitDefaultValue = false)]
        public IList<string> NetworksDeleted { get; set; }
    }
}

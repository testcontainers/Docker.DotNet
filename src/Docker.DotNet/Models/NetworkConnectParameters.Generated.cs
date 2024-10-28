using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class NetworkConnectParameters // (network.ConnectOptions)
    {
        [DataMember(Name = "Container", EmitDefaultValue = false)]
        public string Container { get; set; }

        [DataMember(Name = "EndpointConfig", EmitDefaultValue = false)]
        public EndpointSettings EndpointConfig { get; set; }
    }
}

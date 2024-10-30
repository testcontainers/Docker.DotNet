using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class Commit // (system.Commit)
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public string ID { get; set; }

        [DataMember(Name = "Expected", EmitDefaultValue = false)]
        public string Expected { get; set; }
    }
}

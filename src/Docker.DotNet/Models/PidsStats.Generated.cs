using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class PidsStats // (container.PidsStats)
    {
        [DataMember(Name = "current", EmitDefaultValue = false)]
        public ulong Current { get; set; }

        [DataMember(Name = "limit", EmitDefaultValue = false)]
        public ulong Limit { get; set; }
    }
}

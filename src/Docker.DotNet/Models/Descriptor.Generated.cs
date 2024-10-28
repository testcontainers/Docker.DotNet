using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class Descriptor // (v1.Descriptor)
    {
        [DataMember(Name = "mediaType", EmitDefaultValue = false)]
        public string MediaType { get; set; }

        [DataMember(Name = "digest", EmitDefaultValue = false)]
        public string Digest { get; set; }

        [DataMember(Name = "size", EmitDefaultValue = false)]
        public long Size { get; set; }

        [DataMember(Name = "urls", EmitDefaultValue = false)]
        public IList<string> URLs { get; set; }

        [DataMember(Name = "annotations", EmitDefaultValue = false)]
        public IDictionary<string, string> Annotations { get; set; }

        [DataMember(Name = "platform", EmitDefaultValue = false)]
        public Platform Platform { get; set; }
    }
}

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class ImageProperties // (image.ImageProperties)
    {
        [DataMember(Name = "Platform", EmitDefaultValue = false)]
        public Platform Platform { get; set; }

        [DataMember(Name = "Containers", EmitDefaultValue = false)]
        public IList<string> Containers { get; set; }
    }
}

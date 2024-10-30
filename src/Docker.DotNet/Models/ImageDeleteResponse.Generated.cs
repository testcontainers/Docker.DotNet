using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class ImageDeleteResponse // (image.DeleteResponse)
    {
        [DataMember(Name = "Deleted", EmitDefaultValue = false)]
        public string Deleted { get; set; }

        [DataMember(Name = "Untagged", EmitDefaultValue = false)]
        public string Untagged { get; set; }
    }
}

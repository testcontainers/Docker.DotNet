using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class ImagePushParameters // (main.ImagePushParameters)
    {
        [QueryStringParameter("tag", false)]
        public string Tag { get; set; }

        [QueryStringParameter("platform", false)]
        public string Platform { get; set; }

        [DataMember(Name = "RegistryAuth", EmitDefaultValue = false)]
        public AuthConfig RegistryAuth { get; set; }
    }
}

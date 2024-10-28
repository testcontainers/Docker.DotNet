using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class Platform // (v1.Platform)
    {
        [DataMember(Name = "architecture", EmitDefaultValue = false)]
        public string Architecture { get; set; }

        [DataMember(Name = "os", EmitDefaultValue = false)]
        public string OS { get; set; }

        [DataMember(Name = "os.version", EmitDefaultValue = false)]
        public string OSVersion { get; set; }

        [DataMember(Name = "os.features", EmitDefaultValue = false)]
        public IList<string> OSFeatures { get; set; }

        [DataMember(Name = "variant", EmitDefaultValue = false)]
        public string Variant { get; set; }
    }
}

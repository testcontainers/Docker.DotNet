using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class ManifestSummary // (image.ManifestSummary)
    {
        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public string ID { get; set; }

        [DataMember(Name = "Descriptor", EmitDefaultValue = false)]
        public Descriptor Descriptor { get; set; }

        [DataMember(Name = "Available", EmitDefaultValue = false)]
        public bool Available { get; set; }

        [DataMember(Name = "Kind", EmitDefaultValue = false)]
        public string Kind { get; set; }

        [DataMember(Name = "ImageData", EmitDefaultValue = false)]
        public ImageProperties ImageData { get; set; }

        [DataMember(Name = "AttestationData", EmitDefaultValue = false)]
        public AttestationProperties AttestationData { get; set; }
    }
}

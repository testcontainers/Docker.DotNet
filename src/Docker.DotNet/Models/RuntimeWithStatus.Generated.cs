using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class RuntimeWithStatus // (system.RuntimeWithStatus)
    {
        public RuntimeWithStatus()
        {
        }

        public RuntimeWithStatus(Runtime Runtime)
        {
            if (Runtime != null)
            {
                this.Path = Runtime.Path;
                this.Args = Runtime.Args;
                this.Type = Runtime.Type;
                this.Options = Runtime.Options;
            }
        }

        [DataMember(Name = "path", EmitDefaultValue = false)]
        public string Path { get; set; }

        [DataMember(Name = "runtimeArgs", EmitDefaultValue = false)]
        public IList<string> Args { get; set; }

        [DataMember(Name = "runtimeType", EmitDefaultValue = false)]
        public string Type { get; set; }

        [DataMember(Name = "options", EmitDefaultValue = false)]
        public IDictionary<string, object> Options { get; set; }

        [DataMember(Name = "status", EmitDefaultValue = false)]
        public IDictionary<string, string> Status { get; set; }
    }
}

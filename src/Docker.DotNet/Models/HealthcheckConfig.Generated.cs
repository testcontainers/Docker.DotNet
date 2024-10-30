using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Docker.DotNet.Models
{
    [DataContract]
    public class HealthcheckConfig // (v1.HealthcheckConfig)
    {
        [DataMember(Name = "Test", EmitDefaultValue = false)]
        public IList<string> Test { get; set; }

        [DataMember(Name = "Interval", EmitDefaultValue = false)]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan Interval { get; set; }

        [DataMember(Name = "Timeout", EmitDefaultValue = false)]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan Timeout { get; set; }

        [DataMember(Name = "StartPeriod", EmitDefaultValue = false)]
        public long StartPeriod { get; set; }

        [DataMember(Name = "StartInterval", EmitDefaultValue = false)]
        public long StartInterval { get; set; }

        [DataMember(Name = "Retries", EmitDefaultValue = false)]
        public long Retries { get; set; }
    }
}

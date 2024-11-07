using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class HealthcheckConfig // (v1.HealthcheckConfig)
    {
        [JsonPropertyName("Test")]
        public IList<string> Test { get; set; }

        [JsonPropertyName("Interval")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan Interval { get; set; }

        [JsonPropertyName("Timeout")]
        [JsonConverter(typeof(TimeSpanNanosecondsConverter))]
        public TimeSpan Timeout { get; set; }

        [JsonPropertyName("StartPeriod")]
        public long StartPeriod { get; set; }

        [JsonPropertyName("StartInterval")]
        public long StartInterval { get; set; }

        [JsonPropertyName("Retries")]
        public long Retries { get; set; }
    }
}

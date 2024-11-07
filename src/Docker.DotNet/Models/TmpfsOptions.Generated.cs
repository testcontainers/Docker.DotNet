using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class TmpfsOptions // (mount.TmpfsOptions)
    {
        [JsonPropertyName("SizeBytes")]
        public long SizeBytes { get; set; }

        [JsonPropertyName("Mode")]
        public uint Mode { get; set; }

        [JsonPropertyName("Options")]
        public IList<IList<string>> Options { get; set; }
    }
}

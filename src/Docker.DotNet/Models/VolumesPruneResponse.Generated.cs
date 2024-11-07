using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Docker.DotNet.Models
{
    public class VolumesPruneResponse // (volume.PruneReport)
    {
        [JsonPropertyName("VolumesDeleted")]
        public IList<string> VolumesDeleted { get; set; }

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; }
    }
}

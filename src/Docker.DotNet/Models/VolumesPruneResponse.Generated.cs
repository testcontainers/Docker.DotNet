#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesPruneResponse // (volume.PruneReport)
    {
        [JsonPropertyName("VolumesDeleted")]
        public IList<string> VolumesDeleted { get; set; } = default!;

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; } = default!;
    }
}

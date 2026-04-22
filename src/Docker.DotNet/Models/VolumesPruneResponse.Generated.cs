#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PruneReport contains the response for Engine API:
    /// POST &quot;/volumes/prune&quot;
    /// </summary>
    public class VolumesPruneResponse // (volume.PruneReport)
    {
        [JsonPropertyName("VolumesDeleted")]
        public IList<string> VolumesDeleted { get; set; } = default!;

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; } = default!;
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    /// <summary>
    /// PruneReport contains the response for Engine API:
    /// POST &quot;/containers/prune&quot;
    /// </summary>
    public class ContainersPruneResponse // (container.PruneReport)
    {
        [JsonPropertyName("ContainersDeleted")]
        public IList<string> ContainersDeleted { get; set; } = default!;

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; } = default!;
    }
}

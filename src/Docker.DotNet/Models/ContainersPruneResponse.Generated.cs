#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainersPruneResponse // (container.PruneReport)
    {
        [JsonPropertyName("ContainersDeleted")]
        public IList<string> ContainersDeleted { get; set; } = default!;

        [JsonPropertyName("SpaceReclaimed")]
        public ulong SpaceReclaimed { get; set; } = default!;
    }
}

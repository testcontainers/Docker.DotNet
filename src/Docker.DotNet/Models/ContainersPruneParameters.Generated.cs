#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainersPruneParameters // (main.ContainersPruneParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

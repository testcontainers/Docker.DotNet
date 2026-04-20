#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainersPruneParameters // (main.ContainersPruneParameters)
    {
        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

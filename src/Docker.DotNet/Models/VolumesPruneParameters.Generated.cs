#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesPruneParameters // (main.VolumesPruneParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

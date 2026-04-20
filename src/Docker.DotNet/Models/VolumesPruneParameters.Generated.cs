#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesPruneParameters // (main.VolumesPruneParameters)
    {
        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

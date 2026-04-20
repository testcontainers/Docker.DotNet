#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesPruneParameters // (main.VolumesPruneParameters)
    {
        [QueryStringMapParameter<IDictionary<string, IDictionary<string, bool>>>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

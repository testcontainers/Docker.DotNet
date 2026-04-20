#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesPruneParameters // (main.VolumesPruneParameters)
    {
        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

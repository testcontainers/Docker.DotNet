#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesListParameters // (main.VolumesListParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

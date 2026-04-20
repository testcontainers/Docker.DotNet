#nullable enable
namespace Docker.DotNet.Models
{
    public class VolumesListParameters // (main.VolumesListParameters)
    {
        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainersListParameters // (main.ContainersListParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("all", false)]
        public bool? All { get; set; }

        [QueryStringParameter("limit", false)]
        public long? Limit { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("size", false)]
        public bool? Size { get; set; }

        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

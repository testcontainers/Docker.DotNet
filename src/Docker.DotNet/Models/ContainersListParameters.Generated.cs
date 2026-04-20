#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainersListParameters // (main.ContainersListParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("all", false)]
        public bool? All { get; set; }

        [QueryStringParameter("limit", false)]
        public long? Limit { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("size", false)]
        public bool? Size { get; set; }

        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

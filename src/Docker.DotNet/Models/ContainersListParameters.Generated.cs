#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainersListParameters // (main.ContainersListParameters)
    {
        [QueryStringBoolParameter("all", false)]
        public bool? All { get; set; }

        [QueryStringParameter("limit", false)]
        public long? Limit { get; set; }

        [QueryStringBoolParameter("size", false)]
        public bool? Size { get; set; }

        [QueryStringMapParameter(typeof(IDictionary<string, IDictionary<string, bool>>), "filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

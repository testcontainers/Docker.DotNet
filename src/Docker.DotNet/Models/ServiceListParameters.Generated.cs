#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceListParameters // (main.ServiceListParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("status", false)]
        public bool? Status { get; set; }
    }
}

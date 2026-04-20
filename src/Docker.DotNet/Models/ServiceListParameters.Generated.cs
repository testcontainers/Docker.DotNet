#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceListParameters // (main.ServiceListParameters)
    {
        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("status", false)]
        public bool? Status { get; set; }
    }
}

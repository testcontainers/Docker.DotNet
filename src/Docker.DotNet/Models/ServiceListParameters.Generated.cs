#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceListParameters // (main.ServiceListParameters)
    {
        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringParameter("status", false, typeof(QueryStringBoolConverter))]
        public bool? Status { get; set; }
    }
}

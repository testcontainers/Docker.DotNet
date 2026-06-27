#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceListParameters // (main.ServiceListParameters)
    {
        [QueryStringMapParameter(typeof(IDictionary<string, IDictionary<string, bool>>), "filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringBoolParameter("status", false)]
        public bool? Status { get; set; }
    }
}

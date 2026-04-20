#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworksDeleteUnusedParameters // (main.NetworksDeleteUnusedParameters)
    {
        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

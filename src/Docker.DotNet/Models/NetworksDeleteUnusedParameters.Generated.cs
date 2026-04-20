#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworksDeleteUnusedParameters // (main.NetworksDeleteUnusedParameters)
    {
        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

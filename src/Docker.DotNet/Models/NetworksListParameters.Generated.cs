#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworksListParameters // (main.NetworksListParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

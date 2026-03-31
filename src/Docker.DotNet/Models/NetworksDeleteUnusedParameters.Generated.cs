#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworksDeleteUnusedParameters // (main.NetworksDeleteUnusedParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

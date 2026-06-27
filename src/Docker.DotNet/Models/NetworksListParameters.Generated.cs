#nullable enable
namespace Docker.DotNet.Models
{
    public class NetworksListParameters // (main.NetworksListParameters)
    {
        [QueryStringMapParameter(typeof(IDictionary<string, IDictionary<string, bool>>), "filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

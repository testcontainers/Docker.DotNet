#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginListParameters // (main.PluginListParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

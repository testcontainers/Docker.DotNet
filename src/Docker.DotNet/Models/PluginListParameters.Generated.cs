#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginListParameters // (main.PluginListParameters)
    {
        [QueryStringMapParameter<IDictionary<string, IDictionary<string, bool>>>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

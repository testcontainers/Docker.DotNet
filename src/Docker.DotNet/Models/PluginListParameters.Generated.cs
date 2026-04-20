#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginListParameters // (main.PluginListParameters)
    {
        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

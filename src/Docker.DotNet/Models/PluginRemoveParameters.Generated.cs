#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginRemoveParameters // (main.PluginRemoveParameters)
    {
        [QueryStringParameter("force", false, typeof(QueryStringBoolConverter))]
        public bool? Force { get; set; }
    }
}

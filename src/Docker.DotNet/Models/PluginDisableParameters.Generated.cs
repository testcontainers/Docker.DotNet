#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginDisableParameters // (main.PluginDisableParameters)
    {
        [QueryStringParameter("force", false, typeof(QueryStringBoolConverter))]
        public bool? Force { get; set; }
    }
}

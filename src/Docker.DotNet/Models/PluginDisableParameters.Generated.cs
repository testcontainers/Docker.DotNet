#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginDisableParameters // (main.PluginDisableParameters)
    {
        [QueryStringBoolParameter("force", false)]
        public bool? Force { get; set; }
    }
}

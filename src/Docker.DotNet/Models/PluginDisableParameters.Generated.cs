#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginDisableParameters // (main.PluginDisableParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

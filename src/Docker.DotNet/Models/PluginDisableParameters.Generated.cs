#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginDisableParameters // (main.PluginDisableParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

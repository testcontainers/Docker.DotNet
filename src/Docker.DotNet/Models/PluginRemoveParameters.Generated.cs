#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginRemoveParameters // (main.PluginRemoveParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

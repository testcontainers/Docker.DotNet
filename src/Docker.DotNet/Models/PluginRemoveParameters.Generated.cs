#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginRemoveParameters // (main.PluginRemoveParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

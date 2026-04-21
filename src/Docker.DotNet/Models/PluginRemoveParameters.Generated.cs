#nullable enable
namespace Docker.DotNet.Models
{
    public class PluginRemoveParameters // (main.PluginRemoveParameters)
    {
        [QueryStringBoolParameter("force", false)]
        public bool? Force { get; set; }
    }
}

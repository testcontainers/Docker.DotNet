#nullable enable
namespace Docker.DotNet.Models
{
    public class NodeRemoveParameters // (main.NodeRemoveParameters)
    {
        [QueryStringBoolParameter("force", false)]
        public bool? Force { get; set; }
    }
}

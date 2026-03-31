#nullable enable
namespace Docker.DotNet.Models
{
    public class NodeRemoveParameters // (main.NodeRemoveParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

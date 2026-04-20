#nullable enable
namespace Docker.DotNet.Models
{
    public class NodeRemoveParameters // (main.NodeRemoveParameters)
    {
        [QueryStringParameter("force", false, typeof(QueryStringBoolConverter))]
        public bool? Force { get; set; }
    }
}

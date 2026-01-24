namespace Docker.DotNet.Models
{
    public class NodeRemoveParameters // (main.NodeRemoveParameters)
    {
        [QueryStringParameter("force", false, typeof(BoolQueryStringConverter))]
        public bool? Force { get; set; }
    }
}

namespace Docker.DotNet.Models
{
    public class ContainerResizeParameters // (main.ContainerResizeParameters)
    {
        [QueryStringParameter("h", true)]
        public long Height { get; set; }

        [QueryStringParameter("w", true)]
        public long Width { get; set; }
    }
}

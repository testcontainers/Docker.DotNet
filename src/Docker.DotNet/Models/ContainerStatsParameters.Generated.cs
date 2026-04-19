#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerStatsParameters // (main.ContainerStatsParameters)
    {
        [QueryStringParameter("stream", true, typeof(QueryStringBoolConverter))]
        public bool Stream { get; set; } = true;

        [QueryStringParameter("one-shot", false, typeof(QueryStringBoolConverter))]
        public bool? OneShot { get; set; }
    }
}

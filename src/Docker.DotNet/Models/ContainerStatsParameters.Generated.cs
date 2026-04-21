#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerStatsParameters // (main.ContainerStatsParameters)
    {
        [QueryStringBoolParameter("stream", true)]
        public bool Stream { get; set; } = true;

        [QueryStringBoolParameter("one-shot", false)]
        public bool? OneShot { get; set; }
    }
}

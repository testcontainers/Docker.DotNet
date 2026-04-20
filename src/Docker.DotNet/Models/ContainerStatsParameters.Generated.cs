#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerStatsParameters // (main.ContainerStatsParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("stream", true)]
        public bool Stream { get; set; } = true;

        [QueryStringParameter<QueryStringBoolConverter>("one-shot", false)]
        public bool? OneShot { get; set; }
    }
}

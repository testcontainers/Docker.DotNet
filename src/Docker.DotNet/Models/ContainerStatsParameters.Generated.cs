#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerStatsParameters // (main.ContainerStatsParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("stream", true)]
        public bool Stream { get; set; } = true;

        [QueryStringParameter<BoolQueryStringConverter>("one-shot", false)]
        public bool? OneShot { get; set; }
    }
}

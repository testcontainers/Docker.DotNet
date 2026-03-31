#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerRemoveParameters // (main.ContainerRemoveParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("v", false)]
        public bool? RemoveVolumes { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("link", false)]
        public bool? RemoveLinks { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

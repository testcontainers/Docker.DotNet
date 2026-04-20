#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerRemoveParameters // (main.ContainerRemoveParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("v", false)]
        public bool? RemoveVolumes { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("link", false)]
        public bool? RemoveLinks { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

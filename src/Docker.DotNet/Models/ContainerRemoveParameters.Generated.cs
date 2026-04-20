#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerRemoveParameters // (main.ContainerRemoveParameters)
    {
        [QueryStringBoolParameter("v", false)]
        public bool? RemoveVolumes { get; set; }

        [QueryStringBoolParameter("link", false)]
        public bool? RemoveLinks { get; set; }

        [QueryStringBoolParameter("force", false)]
        public bool? Force { get; set; }
    }
}

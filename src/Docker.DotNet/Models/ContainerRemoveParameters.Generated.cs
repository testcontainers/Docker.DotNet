#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerRemoveParameters // (main.ContainerRemoveParameters)
    {
        [QueryStringParameter("v", false, typeof(QueryStringBoolConverter))]
        public bool? RemoveVolumes { get; set; }

        [QueryStringParameter("link", false, typeof(QueryStringBoolConverter))]
        public bool? RemoveLinks { get; set; }

        [QueryStringParameter("force", false, typeof(QueryStringBoolConverter))]
        public bool? Force { get; set; }
    }
}

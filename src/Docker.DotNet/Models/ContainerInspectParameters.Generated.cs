#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerInspectParameters // (main.ContainerInspectParameters)
    {
        [QueryStringBoolParameter("size", false)]
        public bool? IncludeSize { get; set; }
    }
}

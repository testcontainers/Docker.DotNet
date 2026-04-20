#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerInspectParameters // (main.ContainerInspectParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("size", false)]
        public bool? IncludeSize { get; set; }
    }
}

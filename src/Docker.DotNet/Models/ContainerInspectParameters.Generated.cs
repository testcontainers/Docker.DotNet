#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerInspectParameters // (main.ContainerInspectParameters)
    {
        [QueryStringParameter("size", false, typeof(QueryStringBoolConverter))]
        public bool? IncludeSize { get; set; }
    }
}

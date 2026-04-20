#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmLeaveParameters // (main.SwarmLeaveParameters)
    {
        [QueryStringBoolParameter("force", false)]
        public bool? Force { get; set; }
    }
}

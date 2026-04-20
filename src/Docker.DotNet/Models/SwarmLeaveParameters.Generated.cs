#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmLeaveParameters // (main.SwarmLeaveParameters)
    {
        [QueryStringParameter("force", false, typeof(QueryStringBoolConverter))]
        public bool? Force { get; set; }
    }
}

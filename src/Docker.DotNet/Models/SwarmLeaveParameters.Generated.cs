#nullable enable
namespace Docker.DotNet.Models
{
    public class SwarmLeaveParameters // (main.SwarmLeaveParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("force", false)]
        public bool? Force { get; set; }
    }
}

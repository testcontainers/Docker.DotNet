#nullable enable
namespace Docker.DotNet.Models
{
    public class SytemDataUsageInfoParameters // (main.SytemDataUsageInfoParameters)
    {
        [QueryStringListParameter("type", false)]
        public IList<string>? Type { get; set; }

        [QueryStringBoolParameter("verbose", false)]
        public bool? Verbose { get; set; }
    }
}

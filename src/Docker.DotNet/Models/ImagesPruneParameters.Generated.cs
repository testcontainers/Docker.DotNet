#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesPruneParameters // (main.ImagesPruneParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesPruneParameters // (main.ImagesPruneParameters)
    {
        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

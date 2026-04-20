#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesPruneParameters // (main.ImagesPruneParameters)
    {
        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

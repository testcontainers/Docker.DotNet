#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesSearchParameters // (main.ImagesSearchParameters)
    {
        [QueryStringParameter("term", false)]
        public string? Term { get; set; }

        [QueryStringParameter("limit", false)]
        public long? Limit { get; set; }

        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

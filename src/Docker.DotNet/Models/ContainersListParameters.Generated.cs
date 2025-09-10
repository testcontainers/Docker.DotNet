namespace Docker.DotNet.Models
{
    public class ContainersListParameters // (main.ContainersListParameters)
    {
        [QueryStringParameter("all", false, typeof(BoolQueryStringConverter))]
        public bool? All { get; set; }

        [QueryStringParameter("limit", false)]
        public long? Limit { get; set; }

        [QueryStringParameter("size", false, typeof(BoolQueryStringConverter))]
        public bool? Size { get; set; }

        [QueryStringParameter("filters", false, typeof(MapQueryStringConverter))]
        public IDictionary<string, IDictionary<string, bool>> Filters { get; set; }
    }
}

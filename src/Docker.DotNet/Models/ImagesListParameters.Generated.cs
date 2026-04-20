#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesListParameters // (main.ImagesListParameters)
    {
        [QueryStringParameter("all", false, typeof(QueryStringBoolConverter))]
        public bool? All { get; set; }

        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringParameter("shared-size", false, typeof(QueryStringBoolConverter))]
        public bool? SharedSize { get; set; }

        [QueryStringParameter("digests", false, typeof(QueryStringBoolConverter))]
        public bool? Digests { get; set; }

        [QueryStringParameter("manifests", false, typeof(QueryStringBoolConverter))]
        public bool? Manifests { get; set; }
    }
}

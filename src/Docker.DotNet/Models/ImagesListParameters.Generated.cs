using System.Collections.Generic;

namespace Docker.DotNet.Models
{
    public class ImagesListParameters // (main.ImagesListParameters)
    {
        [QueryStringParameter("all", false, typeof(BoolQueryStringConverter))]
        public bool? All { get; set; }

        [QueryStringParameter("filters", false, typeof(MapQueryStringConverter))]
        public IDictionary<string, IDictionary<string, bool>> Filters { get; set; }

        [QueryStringParameter("shared-size", false, typeof(BoolQueryStringConverter))]
        public bool? SharedSize { get; set; }

        [QueryStringParameter("digests", false, typeof(BoolQueryStringConverter))]
        public bool? Digests { get; set; }

        [QueryStringParameter("manifests", false, typeof(BoolQueryStringConverter))]
        public bool? Manifests { get; set; }
    }
}

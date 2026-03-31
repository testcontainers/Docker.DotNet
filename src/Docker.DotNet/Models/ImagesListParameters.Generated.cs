#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesListParameters // (main.ImagesListParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("all", false)]
        public bool? All { get; set; }

        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("shared-size", false)]
        public bool? SharedSize { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("digests", false)]
        public bool? Digests { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("manifests", false)]
        public bool? Manifests { get; set; }
    }
}

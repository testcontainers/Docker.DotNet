#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesListParameters // (main.ImagesListParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("all", false)]
        public bool? All { get; set; }

        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("shared-size", false)]
        public bool? SharedSize { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("digests", false)]
        public bool? Digests { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("manifests", false)]
        public bool? Manifests { get; set; }
    }
}

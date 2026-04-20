#nullable enable
namespace Docker.DotNet.Models
{
    public class ImagesListParameters // (main.ImagesListParameters)
    {
        [QueryStringBoolParameter("all", false)]
        public bool? All { get; set; }

        [QueryStringMapParameter<IDictionary<string, IDictionary<string, bool>>>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }

        [QueryStringBoolParameter("shared-size", false)]
        public bool? SharedSize { get; set; }

        [QueryStringBoolParameter("digests", false)]
        public bool? Digests { get; set; }

        [QueryStringBoolParameter("manifests", false)]
        public bool? Manifests { get; set; }
    }
}

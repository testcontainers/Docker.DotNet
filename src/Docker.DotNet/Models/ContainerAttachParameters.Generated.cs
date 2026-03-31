#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerAttachParameters // (main.ContainerAttachParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("stream", false)]
        public bool? Stream { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("stdin", false)]
        public bool? Stdin { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("stdout", false)]
        public bool? Stdout { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("stderr", false)]
        public bool? Stderr { get; set; }

        [QueryStringParameter("detachKeys", false)]
        public string? DetachKeys { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("logs", false)]
        public bool? Logs { get; set; }
    }
}

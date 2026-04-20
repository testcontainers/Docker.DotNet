#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerAttachParameters // (main.ContainerAttachParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("stream", false)]
        public bool? Stream { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("stdin", false)]
        public bool? Stdin { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("stdout", false)]
        public bool? Stdout { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("stderr", false)]
        public bool? Stderr { get; set; }

        [QueryStringParameter("detachKeys", false)]
        public string? DetachKeys { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("logs", false)]
        public bool? Logs { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerAttachParameters // (main.ContainerAttachParameters)
    {
        [QueryStringParameter("stream", false, typeof(QueryStringBoolConverter))]
        public bool? Stream { get; set; }

        [QueryStringParameter("stdin", false, typeof(QueryStringBoolConverter))]
        public bool? Stdin { get; set; }

        [QueryStringParameter("stdout", false, typeof(QueryStringBoolConverter))]
        public bool? Stdout { get; set; }

        [QueryStringParameter("stderr", false, typeof(QueryStringBoolConverter))]
        public bool? Stderr { get; set; }

        [QueryStringParameter("detachKeys", false)]
        public string? DetachKeys { get; set; }

        [QueryStringParameter("logs", false, typeof(QueryStringBoolConverter))]
        public bool? Logs { get; set; }
    }
}

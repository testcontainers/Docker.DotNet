#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerAttachParameters // (main.ContainerAttachParameters)
    {
        [QueryStringBoolParameter("stream", false)]
        public bool? Stream { get; set; }

        [QueryStringBoolParameter("stdin", false)]
        public bool? Stdin { get; set; }

        [QueryStringBoolParameter("stdout", false)]
        public bool? Stdout { get; set; }

        [QueryStringBoolParameter("stderr", false)]
        public bool? Stderr { get; set; }

        [QueryStringParameter("detachKeys", false)]
        public string? DetachKeys { get; set; }

        [QueryStringBoolParameter("logs", false)]
        public bool? Logs { get; set; }
    }
}

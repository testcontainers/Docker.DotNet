#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerLogsParameters // (main.ContainerLogsParameters)
    {
        [QueryStringBoolParameter("stdout", false)]
        public bool? ShowStdout { get; set; }

        [QueryStringBoolParameter("stderr", false)]
        public bool? ShowStderr { get; set; }

        [QueryStringParameter("since", false)]
        public string? Since { get; set; }

        [QueryStringParameter("until", false)]
        public string? Until { get; set; }

        [QueryStringBoolParameter("timestamps", false)]
        public bool? Timestamps { get; set; }

        [QueryStringBoolParameter("follow", false)]
        public bool? Follow { get; set; }

        [QueryStringParameter("tail", false)]
        public string? Tail { get; set; }
    }
}

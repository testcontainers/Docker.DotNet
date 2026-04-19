#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerLogsParameters // (main.ContainerLogsParameters)
    {
        [QueryStringParameter("stdout", false, typeof(QueryStringBoolConverter))]
        public bool? ShowStdout { get; set; }

        [QueryStringParameter("stderr", false, typeof(QueryStringBoolConverter))]
        public bool? ShowStderr { get; set; }

        [QueryStringParameter("since", false)]
        public string? Since { get; set; }

        [QueryStringParameter("until", false)]
        public string? Until { get; set; }

        [QueryStringParameter("timestamps", false, typeof(QueryStringBoolConverter))]
        public bool? Timestamps { get; set; }

        [QueryStringParameter("follow", false, typeof(QueryStringBoolConverter))]
        public bool? Follow { get; set; }

        [QueryStringParameter("tail", false)]
        public string? Tail { get; set; }
    }
}

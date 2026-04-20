#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerLogsParameters // (main.ContainerLogsParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("stdout", false)]
        public bool? ShowStdout { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("stderr", false)]
        public bool? ShowStderr { get; set; }

        [QueryStringParameter("since", false)]
        public string? Since { get; set; }

        [QueryStringParameter("until", false)]
        public string? Until { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("timestamps", false)]
        public bool? Timestamps { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("follow", false)]
        public bool? Follow { get; set; }

        [QueryStringParameter("tail", false)]
        public string? Tail { get; set; }
    }
}

#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceLogsParameters // (main.ServiceLogsParameters)
    {
        [QueryStringParameter<QueryStringBoolConverter>("stdout", false)]
        public bool? ShowStdout { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("stderr", false)]
        public bool? ShowStderr { get; set; }

        [QueryStringParameter("since", false)]
        public string? Since { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("timestamps", false)]
        public bool? Timestamps { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("follow", false)]
        public bool? Follow { get; set; }

        [QueryStringParameter("tail", false)]
        public string? Tail { get; set; }

        [QueryStringParameter<QueryStringBoolConverter>("details", false)]
        public bool? Details { get; set; }
    }
}

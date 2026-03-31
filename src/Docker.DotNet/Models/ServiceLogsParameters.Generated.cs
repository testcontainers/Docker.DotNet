#nullable enable
namespace Docker.DotNet.Models
{
    public class ServiceLogsParameters // (main.ServiceLogsParameters)
    {
        [QueryStringParameter<BoolQueryStringConverter>("stdout", false)]
        public bool? ShowStdout { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("stderr", false)]
        public bool? ShowStderr { get; set; }

        [QueryStringParameter("since", false)]
        public string? Since { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("timestamps", false)]
        public bool? Timestamps { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("follow", false)]
        public bool? Follow { get; set; }

        [QueryStringParameter("tail", false)]
        public string? Tail { get; set; }

        [QueryStringParameter<BoolQueryStringConverter>("details", false)]
        public bool? Details { get; set; }
    }
}

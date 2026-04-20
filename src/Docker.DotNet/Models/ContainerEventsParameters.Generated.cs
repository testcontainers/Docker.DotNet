#nullable enable
namespace Docker.DotNet.Models
{
    public class ContainerEventsParameters // (main.ContainerEventsParameters)
    {
        [QueryStringParameter("since", false)]
        public string? Since { get; set; }

        [QueryStringParameter("until", false)]
        public string? Until { get; set; }

        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

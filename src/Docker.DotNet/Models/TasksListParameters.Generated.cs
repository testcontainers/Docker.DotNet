#nullable enable
namespace Docker.DotNet.Models
{
    public class TasksListParameters // (main.TasksListParameters)
    {
        [QueryStringParameter<MapQueryStringConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

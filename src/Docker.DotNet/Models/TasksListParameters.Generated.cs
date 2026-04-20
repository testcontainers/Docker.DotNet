#nullable enable
namespace Docker.DotNet.Models
{
    public class TasksListParameters // (main.TasksListParameters)
    {
        [QueryStringParameter<QueryStringMapConverter>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

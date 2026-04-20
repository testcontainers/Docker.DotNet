#nullable enable
namespace Docker.DotNet.Models
{
    public class TasksListParameters // (main.TasksListParameters)
    {
        [QueryStringParameter("filters", false, typeof(QueryStringMapConverter))]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

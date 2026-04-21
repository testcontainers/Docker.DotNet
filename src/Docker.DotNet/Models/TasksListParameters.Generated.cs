#nullable enable
namespace Docker.DotNet.Models
{
    public class TasksListParameters // (main.TasksListParameters)
    {
        [QueryStringMapParameter<IDictionary<string, IDictionary<string, bool>>>("filters", false)]
        public IDictionary<string, IDictionary<string, bool>>? Filters { get; set; }
    }
}

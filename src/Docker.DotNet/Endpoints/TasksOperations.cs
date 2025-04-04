namespace Docker.DotNet;

internal class TasksOperations : ITasksOperations
{
    private readonly DockerClient _client;

    internal TasksOperations(DockerClient client)
    {
        _client = client;
    }

    Task<IList<TaskResponse>> ITasksOperations.ListAsync(CancellationToken cancellationToken)
    {
        return ((ITasksOperations)this).ListAsync(null, cancellationToken);
    }

    async Task<IList<TaskResponse>> ITasksOperations.ListAsync(TasksListParameters parameters, CancellationToken cancellationToken)
    {
        IQueryString query = null;
        if (parameters != null) {
            query = new QueryString<TasksListParameters>(parameters);
        }

        return await _client.MakeRequestAsync<IList<TaskResponse>>(_client.NoErrorHandlers, HttpMethod.Get, "tasks", query, cancellationToken).ConfigureAwait(false);
    }

    async Task<TaskResponse> ITasksOperations.InspectAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<TaskResponse>(_client.NoErrorHandlers, HttpMethod.Get, $"tasks/{id}", cancellationToken).ConfigureAwait(false);
    }
}
namespace Docker.DotNet;

internal class TasksOperations : ITasksOperations
{
    private readonly DockerClient _client;

    internal TasksOperations(DockerClient client)
    {
        _client = client;
    }

    public Task<IList<TaskResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        return ListAsync(null, cancellationToken);
    }

    public async Task<IList<TaskResponse>> ListAsync(TasksListParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = parameters == null ? null : new QueryString<TasksListParameters>(parameters);

        return await _client.MakeRequestAsync<IList<TaskResponse>>(_client.NoErrorHandlers, HttpMethod.Get, "tasks", queryParameters, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<TaskResponse> InspectAsync(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            throw new ArgumentNullException(nameof(id));
        }

        return await _client.MakeRequestAsync<TaskResponse>(_client.NoErrorHandlers, HttpMethod.Get, $"tasks/{id}", cancellationToken)
            .ConfigureAwait(false);
    }
}
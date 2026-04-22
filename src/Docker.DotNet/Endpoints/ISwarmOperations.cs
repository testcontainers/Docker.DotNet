namespace Docker.DotNet;

public interface ISwarmOperations
{
    #region Swarm

    /// <summary>
    /// Get the unlock key.
    /// </summary>
    /// <remarks>
    /// 200 - No Error.
    /// 500 - Server Error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<SwarmUnlockResponse> GetSwarmUnlockKeyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Initialize a new swarm.
    /// </summary>
    /// <remarks>
    /// 200 - No Error.
    /// 400 - Bad parameters.
    /// 500 - Server Error.
    /// 503 - Node is already part of a swarm.
    /// </remarks>
    /// <param name="parameters">The join parameters.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>The node id.</returns>
    Task<string> InitSwarmAsync(SwarmInitParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inspect swarm.
    /// </summary>
    /// <remarks>
    /// 200 - No Error.
    /// 404 - No such swarm.
    /// 500 - Server Error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<SwarmInspectResponse> InspectSwarmAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Join an existing swarm.
    /// </summary>
    /// <remarks>
    /// 200 - No Error.
    /// 500 - Server Error.
    /// 503 - Node is already part of a swarm.
    /// </remarks>
    /// <param name="parameters">The join parameters.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task JoinSwarmAsync(SwarmJoinParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Leave a swarm.
    /// </summary>
    /// <remarks>
    /// 200 - No Error.
    /// 500 - Server Error.
    /// 503 - Node not part of a swarm.
    /// </remarks>
    /// <param name="parameters">The leave parameters.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task LeaveSwarmAsync(SwarmLeaveParameters? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unlock a locked manager.
    /// </summary>
    /// <remarks>
    /// 200 - No Error.
    /// 500 - Server Error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="parameters">The swarm's unlock key.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task UnlockSwarmAsync(SwarmUnlockParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a swarm.
    /// </summary>
    /// <remarks>
    /// 200 - No Error.
    /// 400 - Bad parameter.
    /// 500 - Server Error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="parameters">The update parameters.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task UpdateSwarmAsync(SwarmUpdateParameters parameters, CancellationToken cancellationToken = default);

    #endregion Swarm

    #region Services

    /// <summary>
    /// Create a service.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 400 - Bad parameter.
    /// 403 - Network is not eligible for services.
    /// 409 - Name conflicts with an existing service.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<ServiceCreateResponse> CreateServiceAsync(ServiceCreateParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Inspect a service.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 404 - No such service.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="id">ID or name of service.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>The service spec.</returns>
    Task<SwarmService> InspectServiceAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// List services with optional serviceFilters.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<IList<SwarmService>> ListServicesAsync(ServiceListParameters? parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a service.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 400 - Bad parameter.
    /// 404 - No such service.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <param name="id">ID or name of service.</param>
    /// <returns>The service spec.</returns>
    Task<ServiceUpdateResponse> UpdateServiceAsync(string id, ServiceUpdateParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a service.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 404 - No such service.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="id">ID or name of service.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task RemoveServiceAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets <c>stdout</c> and <c>stderr</c> logs from services.
    /// </summary>
    /// <param name="id">The ID or name of the service.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that will complete once all log lines have been read.</returns>
    /// <remarks>
    /// This method is only suited for services with the <c>json-file</c> or <c>journald</c> logging driver.
    ///
    /// HTTP GET /services/(id)/logs
    ///
    /// 101 - Logs returned as a stream.
    /// 200 - Logs returned as a string in response body.
    /// 404 - No such service.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    Task<Stream> GetServiceLogsAsync(string id, ServiceLogsParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets <c>stdout</c> and <c>stderr</c> logs from services.
    /// </summary>
    /// <param name="id">The ID or name of the service.</param>
    /// <param name="tty">Indicates whether the service was created with a TTY. If <see langword="false"/>, the returned stream is multiplexed.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to a <see cref="MultiplexedStream"/>, which provides the log information.
    /// If the service wasn't created with a TTY, this stream is multiplexed.
    /// </returns>
    /// <remarks>
    /// This method is only suited for services with the <c>json-file</c> or <c>journald</c> logging driver.
    /// </remarks>
    Task<MultiplexedStream> GetServiceLogsAsync(string id, bool tty, ServiceLogsParameters parameters, CancellationToken cancellationToken = default);

    #endregion Services

    #region Nodes

    /// <summary>
    /// List nodes.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<IList<NodeListResponse>> ListNodesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Inspect a node.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 404 - No such node.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task<NodeListResponse> InspectNodeAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a node.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 404 - No such node.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task RemoveNodeAsync(string id, bool? force = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update node.
    /// </summary>
    /// <remarks>
    /// 200 - No error.
    /// 404 - No such node.
    /// 500 - Server error.
    /// 503 - Node is not part of a swarm.
    /// </remarks>
    /// <param name="id">ID or name of the node.</param>
    /// <param name="version">Current version of the node object.</param>
    /// <param name="parameters">Parameters to update.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    Task UpdateNodeAsync(string id, ulong version, NodeUpdateParameters parameters, CancellationToken cancellationToken = default);

    #endregion
}
namespace Docker.DotNet;

/// <summary>
/// Provides operations relating to Docker containers.
/// </summary>
public interface IContainerOperations
{
    /// <summary>
    /// Returns a list of containers.
    /// </summary>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a list of <see cref="ContainerListResponse"/> objects, which represent the containers found.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker ps</c> and <c>docker container ls</c>.</remarks>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<IList<ContainerListResponse>> ListContainersAsync(ContainersListParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new container from an image.
    /// </summary>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="CreateContainerResponse"/>, which provides information about the newly-created container.</returns>
    /// <remarks>The corresponding command in the Docker CLI is <c>docker container create</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid, there was a conflict with another container, or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<CreateContainerResponse> CreateContainerAsync(CreateContainerParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves low-level information about a container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="ContainerInspectResponse"/>, which holds details about the container.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker inspect</c> and <c>docker container inspect</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<ContainerInspectResponse> InspectContainerAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves low-level information about a container with additional options.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation, such as whether to include size information.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="ContainerInspectResponse"/>, which holds details about the container.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker inspect --size</c> and <c>docker container inspect --size</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<ContainerInspectResponse> InspectContainerAsync(string id, ContainerInspectParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of processes running within the container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="ContainerProcessesResponse"/>, which holds information about the processes.</returns>
    /// <remarks>This operation is not supported on Windows, because the underlying API does not support it.
    /// <br/>The corresponding commands in the Docker CLI are <c>docker top</c> and <c>docker container top</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<ContainerProcessesResponse> ListProcessesAsync(string id, ContainerListProcessesParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets <c>stdout</c> and <c>stderr</c> logs from a container.
    /// </summary>
    /// <remarks>
    /// The corresponding commands in the Docker CLI are <c>docker inspect</c> and <c>docker container inspect</c>.
    /// </remarks>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="progress">Provides a callback for reporting log entries as they're read. Every reported string represents one log line, with its terminating newline removed.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>
    /// A <see cref="Task"/> that will complete once all log lines have been read, or once the container has exited if <c>Follow</c> is set to <see langword="true"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">One or more of the inputs were <see langword="null"/>.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation, or timeout.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    Task GetContainerLogsAsync(string id, ContainerLogsParameters parameters, IProgress<string> progress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets <c>stdout</c> and <c>stderr</c> logs from a container.
    /// </summary>
    /// <remarks>
    /// The corresponding commands in the Docker CLI are <c>docker inspect</c> and <c>docker container inspect</c>.
    /// </remarks>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to a <see cref="MultiplexedStream"/>, which provides the log information.
    /// </returns>
    /// <exception cref="ArgumentNullException">One or more of the inputs were <see langword="null"/>.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation, or timeout.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    Task<MultiplexedStream> GetContainerLogsAsync(string id, ContainerLogsParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reports which files in a container's filesystem have been added, deleted, or modified.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a list of <see cref="ContainerFileSystemChangeResponse"/> objects. Each object corresponds to a single changed file.</returns>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<IList<ContainerFileSystemChangeResponse>> InspectChangesAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports the contents of a container as a tarball.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="Stream"/>, which can be read to obtain the bytes of the tarball.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker export</c> and <c>docker container export</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<Stream> ExportContainerAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a live, raw stream of the container's resource usage statistics.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="Stream"/>, which can be used to read the frames of statistics. For details
    /// on the format, refer to <a href="https://docs.docker.com/engine/api/v1.41/#operation/ContainerStats">the Docker Engine API documentation</a>.
    /// </returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker stats</c> and <c>docker container stats</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    [Obsolete("Use 'Task GetContainerStatsAsync(string id, ContainerStatsParameters parameters, CancellationToken cancellationToken, IProgress<JSONMessage> progress)'")]
    Task<Stream> GetContainerStatsAsync(string id, ContainerStatsParameters parameters, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a live, raw stream of the container's resource usage statistics.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="progress">Provides a callback to trigger whenever a new frame of statistics is available.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <remarks>A <see cref="Task"/> that resolves when the stream has been established.</remarks>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker stats</c> and <c>docker container stats</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task GetContainerStatsAsync(string id, ContainerStatsParameters parameters, IProgress<ContainerStatsResponse> progress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resizes a container's TTY.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    /// <remarks>You must restart the container for the change to take effect.</remarks>
    /// <seealso cref="RestartContainerAsync(string, ContainerRestartParameters, CancellationToken)"/>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task ResizeContainerTtyAsync(string id, ContainerResizeParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts a container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a value indicating whether the container was successfully started.</returns>
    /// <remarks>This method does not report an error if the container is already started.
    /// <br/>The corresponding commands in the Docker CLI are <c>docker start</c> and <c>docker container start</c>.</remarks>
    /// <seealso cref="StopContainerAsync(string, ContainerStopParameters, CancellationToken)"/>
    /// <seealso cref="RestartContainerAsync(string, ContainerRestartParameters, CancellationToken)"/>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<bool> StartContainerAsync(string id, ContainerStartParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops a container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a value indicating whether the container was successfully stopped.</returns>
    /// <remarks>This method does not report an error if the container is already stopped.
    /// <br/>The corresponding commands in the Docker CLI are <c>docker stop</c> and <c>docker container stop</c>.</remarks>
    /// <seealso cref="StartContainerAsync(string, ContainerStartParameters, CancellationToken)"/>
    /// <seealso cref="KillContainerAsync(string, ContainerKillParameters, CancellationToken)"/>
    /// <seealso cref="WaitContainerAsync(string, CancellationToken)"/>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<bool> StopContainerAsync(string id, ContainerStopParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops and then restarts a container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker restart</c> and <c>docker container restart</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task RestartContainerAsync(string id, ContainerRestartParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a POSIX signal to a container--typically to kill it.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker kill</c> and <c>docker container kill</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <seealso cref="StopContainerAsync(string, ContainerStopParameters, CancellationToken)"/>
    /// <seealso cref="PruneContainersAsync(ContainersPruneParameters, CancellationToken)"/>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The container was not running, the input is invalid, or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task KillContainerAsync(string id, ContainerKillParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes the name of a container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The name is already in use, the input is invalid, or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task RenameContainerAsync(string id, ContainerRenameParameters parameters, CancellationToken cancellationToken);

    /// <summary>
    /// Suspends a container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    /// <remarks>
    /// This uses the freeze cgroup to suspend all processes in the container. The processes are unaware that they are being
    /// suspended (e.g., they cannot capture a SIGSTOP signal).
    /// </remarks>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker pause</c> and <c>docker container pause</c>.</remarks>
    /// <seealso cref="UnpauseContainerAsync(string, CancellationToken)"/>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task PauseContainerAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resumes a container that was suspended.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation is complete.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker unpause</c> and <c>docker container unpause</c>.</remarks>
    /// <seealso cref="PauseContainerAsync(string, CancellationToken)"/>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task UnpauseContainerAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attaches to a container to read its output and send it input.
    /// </summary>
    /// <remarks>
    /// The corresponding commands in the Docker CLI are <c>docker attach</c> and <c>docker container attach</c>.
    /// </remarks>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that resolves to a <see cref="MultiplexedStream"/>, which contains the container's
    /// <c>stdout</c> and <c>stderr</c> content and which can be used to write to the container's <c>stdin</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">One or more of the inputs were <see langword="null"/>.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation, or timeout.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    Task<MultiplexedStream> AttachContainerAsync(string id, ContainerAttachParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Waits for a container to stop.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="ContainerWaitResponse"/> when the container has
    /// stopped.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker wait</c> and <c>docker container wait</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<ContainerWaitResponse> WaitContainerAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a container.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the container has been removed.</returns>
    /// <remarks>The corresponding commands in the Docker CLI are <c>docker rm</c> and <c>docker container rm</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">There is a conflict, the input is invalid, or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task RemoveContainerAsync(string id, ContainerRemoveParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about the filesystem in a container. This may be either a listing of files or a complete
    /// representation of the filesystem.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="statOnly">If <see langword="true"/>, the method will only return file information; otherwise, it will return a
    /// stream of the filesystem as a tarball.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="ContainerArchiveResponse"/>, which holds
    /// either the files or a <see cref="Stream"/> if the tarball.</returns>
    /// <exception cref="DockerContainerNotFoundException">No such container was found, or the path does not exist.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<ContainerArchiveResponse> GetArchiveFromContainerAsync(string id, ContainerPathStatParameters parameters, bool statOnly, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts a tar archive into a container's filesystem.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="stream">A readable stream containing the tarball.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task"/> that resolves when the operation completes.</returns>
    /// <exception cref="DockerContainerNotFoundException">No such container was found, or the path does not exist inside the container.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">Permission is denied (the volume or container rootfs is marked read-only),
    /// the input is invalid, or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task ExtractArchiveToContainerAsync(string id, ContainerPathStatParameters parameters, Stream stream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes stopped containers.
    /// </summary>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="ContainersPruneResponse"/>, which details which containers
    /// were removed.</returns>
    /// <remarks>The corresponding command in the Docker CLI is <c>docker container prune</c>.</remarks>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<ContainersPruneResponse> PruneContainersAsync(ContainersPruneParameters parameters = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes configuration options of a container without recreating it.
    /// </summary>
    /// <param name="id">The ID or name of the container.</param>
    /// <param name="parameters">Specifics of how to perform the operation.</param>
    /// <param name="cancellationToken">When triggered, the operation will stop at the next available time, if possible.</param>
    /// <returns>A <see cref="Task{TResult}"/> that resolves to a <see cref="ContainerUpdateResponse"/>, which provides updated
    /// information about the container.</returns>
    /// <remarks>The corresponding command in the Docker CLI is <c>docker update</c>.</remarks>
    /// <exception cref="DockerContainerNotFoundException">No such container was found.</exception>
    /// <exception cref="ArgumentNullException">One or more of the inputs was <see langword="null"/>.</exception>
    /// <exception cref="DockerApiException">The input is invalid or the daemon experienced an error.</exception>
    /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
    Task<ContainerUpdateResponse> UpdateContainerAsync(string id, ContainerUpdateParameters parameters, CancellationToken cancellationToken = default);
}
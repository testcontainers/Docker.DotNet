namespace Docker.DotNet;

public interface IExecOperations
{
    Task<ContainerExecInspectResponse> InspectContainerExecAsync(string id, CancellationToken cancellationToken = default);

    Task<ContainerExecCreateResponse> CreateContainerExecAsync(string id, ContainerExecCreateParameters parameters, CancellationToken cancellationToken = default);

    Task<MultiplexedStream> StartContainerExecAsync(string id, ContainerExecStartParameters parameters, CancellationToken cancellationToken = default);

    Task ResizeExecTtyAsync(string id, ContainerResizeParameters parameters, CancellationToken cancellationToken = default);
}
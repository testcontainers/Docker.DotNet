namespace Docker.DotNet;

public interface IDockerClient : IDisposable
{
    ISystemOperations System { get; }

    IContainerOperations Containers { get; }

    IImageOperations Images { get; }

    INetworkOperations Networks { get; }

    IVolumeOperations Volumes { get; }

    ISecretsOperations Secrets { get; }

    IConfigOperations Configs { get; }

    ISwarmOperations Swarm { get; }

    ITasksOperations Tasks { get; }

    IPluginOperations Plugin { get; }

    IExecOperations Exec { get; }
}
namespace Docker.DotNet.Models;

public sealed record ContainerArchiveResponse
{
    public ContainerPathStatResponse Stat { get; set; } = null!;

    public Stream? Stream { get; set; }
}
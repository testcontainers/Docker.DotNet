namespace Docker.DotNet.Models;

public class ContainerArchiveResponse
{
    public ContainerPathStatResponse Stat { get; set; }

    public Stream Stream { get; set; }
}
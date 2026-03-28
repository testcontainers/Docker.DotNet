namespace Docker.DotNet.Benchmarks;

[MemoryDiagnoser]
public class DockerDaemonRoundtripBenchmarks
{
    private readonly string _imageReference = "busybox:1.37";

    private DockerClient _client = null!;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
#if DOCKER_DOTNET_RELEASE
        const string implementation = "release";
#else
        const string implementation = "main";
#endif

        var builder = new DockerClientBuilder();

        _client = builder.Build();

        Console.WriteLine($"Running daemon round-trip benchmarks against: {implementation}");

        await _client.System.PingAsync(CancellationToken.None)
            .ConfigureAwait(false);

        await EnsureImageExistsAsync()
            .ConfigureAwait(false);
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _client.Dispose();
    }

    [Benchmark]
    public async Task<int> CreateContainerRequestResponse()
    {
        var parameters = new CreateContainerParameters();
        parameters.Name = $"dockerdotnet-benchmark-{Guid.NewGuid():N}";
        parameters.Image = _imageReference;
        parameters.Cmd = ["sh", "-c", "sleep 1"];
        parameters.Labels = new Dictionary<string, string>();
        parameters.Labels.Add("suite", "benchmark");
        parameters.Labels.Add("scenario", "create-roundtrip");

        var response = await _client.Containers.CreateContainerAsync(parameters, CancellationToken.None)
            .ConfigureAwait(false);

        try
        {
            return response.ID.Length;
        }
        finally
        {
            await SafeRemoveContainerAsync(response.ID)
                .ConfigureAwait(false);
        }
    }

    [Benchmark]
    public async Task<bool> StartContainerRequestResponse()
    {
        var parameters = new CreateContainerParameters();
        parameters.Name = $"dockerdotnet-benchmark-{Guid.NewGuid():N}";
        parameters.Image = _imageReference;
        parameters.Cmd = ["sh", "-c", "exit 0"];
        parameters.Labels = new Dictionary<string, string>();
        parameters.Labels.Add("suite", "benchmark");
        parameters.Labels.Add("scenario", "start-roundtrip");

        var response = await _client.Containers.CreateContainerAsync(parameters, CancellationToken.None)
            .ConfigureAwait(false);

        try
        {
            return await _client.Containers.StartContainerAsync(response.ID, new ContainerStartParameters(), CancellationToken.None)
                .ConfigureAwait(false);
        }
        finally
        {
            await SafeRemoveContainerAsync(response.ID)
                .ConfigureAwait(false);
        }
    }

    private async Task EnsureImageExistsAsync()
    {
        var parameters = new ImagesCreateParameters();
        parameters.FromImage = _imageReference;

        await _client.Images.CreateImageAsync(parameters, new AuthConfig(), new Progress<JSONMessage>(_ => { }), CancellationToken.None)
            .ConfigureAwait(false);
    }

    private async Task SafeRemoveContainerAsync(string containerId)
    {
        var parameters = new ContainerRemoveParameters();
        parameters.Force = true;

        try
        {
            await _client.Containers.RemoveContainerAsync(containerId, parameters, CancellationToken.None)
                .ConfigureAwait(false);
        }
        catch (DockerContainerNotFoundException)
        {
        }
    }
}
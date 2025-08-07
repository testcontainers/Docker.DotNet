using Docker.DotNet;

public sealed class GitHub11(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task Test()
    {
        const string image = "postgres:17";
        const string database = "foo";
        const string username = "bar";
        const string password = "baz";

        IList<string> envs = new List<string>();
        envs.Add("POSTGRES_DB=" + database);
        envs.Add("POSTGRES_USER=" + username);
        envs.Add("POSTGRES_PASSWORD=" + password);

        var command = new[] { "pg_isready", "--host", "localhost", "--dbname", database, "--username", username };

        using var dockerClientConfiguration = new DockerClientConfiguration(
            endpoint: new Uri("unix:///tmp/storage-run-1000/podman/podman.sock"));
        using var dockerClient = dockerClientConfiguration.CreateClient();

        // Pull the image once to avoid: You have reached your pull rate limit.
        // await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = image }, new AuthConfig(), new Progress<JSONMessage>());
        var createResponse = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters { Image = image, Env = envs });
        await dockerClient.Containers.StartContainerAsync(createResponse.ID, new ContainerStartParameters());

        foreach (var _ in Enumerable.Range(0, 100))
        {
            var execResponse = await dockerClient.Exec.CreateContainerExecAsync(createResponse.ID, new ContainerExecCreateParameters { Cmd = command, AttachStdout = true, AttachStderr = true, AttachStdin = false });
            using var stream = await dockerClient.Exec.StartContainerExecAsync(execResponse.ID,  new ContainerExecStartParameters());
            var (stdout, stderr) = await stream.ReadOutputToEndAsync(CancellationToken.None);
            var execInspectResponse = await dockerClient.Exec.InspectContainerExecAsync(execResponse.ID);
            testOutputHelper.WriteLine($"ExitCode={execInspectResponse.ExitCode} Stdout='{stdout.Trim()}' Stderr='{stderr.Trim()}'");
        }
    }
}
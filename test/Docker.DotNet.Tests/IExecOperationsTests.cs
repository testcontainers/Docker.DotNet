namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class IExecOperationsTests
{
    private readonly TestFixture _testFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public IExecOperationsTests(TestFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ResizeExecTtyAsync_RunningExec_Succeeds()
    {
        // Given: a running container with a TTY exec session
        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Entrypoint = CommonCommands.SleepInfinity
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        var execCreateResponse = await _testFixture.DockerClient.Exec.CreateContainerExecAsync(
            createContainerResponse.ID,
            new ContainerExecCreateParameters
            {
                AttachStdout = true,
                AttachStderr = true,
                AttachStdin = true,
                TTY = true,
                Cmd = ["/bin/sh"]
            },
            _testFixture.Cts.Token
        );

        // Start the exec to make it running (resize only works on a running exec)
        using var stream = await _testFixture.DockerClient.Exec.StartContainerExecAsync(
            execCreateResponse.ID,
            new ContainerExecStartParameters { TTY = true },
            _testFixture.Cts.Token
        );

        // When: resize the exec TTY
        await _testFixture.DockerClient.Exec.ResizeExecTtyAsync(
            execCreateResponse.ID,
            new ContainerResizeParameters
            {
                Height = 40,
                Width = 120
            },
            _testFixture.Cts.Token
        );

        // Then: no exception means success
        _testOutputHelper.WriteLine("ResizeExecTtyAsync succeeded for running exec session.");

        // Verify exec is still running
        var execInspect = await _testFixture.DockerClient.Exec.InspectContainerExecAsync(
            execCreateResponse.ID,
            _testFixture.Cts.Token
        );

        Assert.True(execInspect.Running);
    }

    [Fact]
    public async Task ResizeExecTtyAsync_NonExistentExecId_ThrowsException()
    {
        // When/Then: resizing a non-existent exec ID should throw
        await Assert.ThrowsAsync<DockerContainerNotFoundException>(
            () => _testFixture.DockerClient.Exec.ResizeExecTtyAsync(
                Guid.NewGuid().ToString("N"),
                new ContainerResizeParameters
                {
                    Height = 24,
                    Width = 80
                },
                _testFixture.Cts.Token
            )
        );
    }
}

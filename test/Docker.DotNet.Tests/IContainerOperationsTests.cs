namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class IContainerOperationsTests
{
    private readonly TestFixture _testFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public IContainerOperationsTests(TestFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateContainerAsync_CreatesContainer()
    {
        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
            },
            _testFixture.Cts.Token
        );

        Assert.NotNull(createContainerResponse);
        Assert.NotEmpty(createContainerResponse.ID);
    }

    [Fact]
    public async Task GetContainerLogs_Tty_False_Follow_True_TaskIsCompleted()
    {
        using var containerLogsCts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = false
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        containerLogsCts.CancelAfter(TimeSpan.FromSeconds(5));

        var containerLogsTask = _testFixture.DockerClient.Containers.GetContainerLogsAsync(
            createContainerResponse.ID,
            new ContainerLogsParameters
            {
                ShowStderr = true,
                ShowStdout = true,
                Timestamps = true,
                Follow = true
            },
            containerLogsCts.Token,
            new Progress<string>(m => _testOutputHelper.WriteLine(m))
        );

        await _testFixture.DockerClient.Containers.StopContainerAsync(
            createContainerResponse.ID,
            new ContainerStopParameters(),
            _testFixture.Cts.Token
        );

        await containerLogsTask;
        Assert.True(containerLogsTask.IsCompletedSuccessfully);
    }

    [Fact]
    public async Task GetContainerLogs_Tty_False_Follow_False_ReadsLogs()
    {
        var logList = new List<string>();

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = false
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        await Task.Delay(TimeSpan.FromSeconds(5));

        await _testFixture.DockerClient.Containers.GetContainerLogsAsync(
            createContainerResponse.ID,
            new ContainerLogsParameters
            {
                ShowStderr = true,
                ShowStdout = true,
                Timestamps = true,
                Follow = false
            },
            _testFixture.Cts.Token,
            new Progress<string>(m => { logList.Add(m); _testOutputHelper.WriteLine(m); })
        );

        await _testFixture.DockerClient.Containers.StopContainerAsync(
            createContainerResponse.ID,
            new ContainerStopParameters(),
            _testFixture.Cts.Token
        );

        _testOutputHelper.WriteLine($"Line count: {logList.Count}");

        Assert.NotEmpty(logList);
    }

    [Fact]
    public async Task GetContainerLogs_Tty_True_Follow_False_ReadsLogs()
    {
        var logList = new List<string>();

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = true
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        await Task.Delay(TimeSpan.FromSeconds(5));

        await _testFixture.DockerClient.Containers.GetContainerLogsAsync(
            createContainerResponse.ID,
            new ContainerLogsParameters
            {
                ShowStderr = true,
                ShowStdout = true,
                Timestamps = true,
                Follow = false
            },
            _testFixture.Cts.Token,
            new Progress<string>(m => { _testOutputHelper.WriteLine(m); logList.Add(m); })
        );

        await _testFixture.DockerClient.Containers.StopContainerAsync(
            createContainerResponse.ID,
            new ContainerStopParameters(),
            _testFixture.Cts.Token
        );

        _testOutputHelper.WriteLine($"Line count: {logList.Count}");

        Assert.NotEmpty(logList);
    }

    [Fact]
    public async Task GetContainerLogs_Tty_False_Follow_True_Requires_Task_To_Be_Cancelled()
    {
        using var containerLogsCts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = false
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        containerLogsCts.CancelAfter(TimeSpan.FromSeconds(5));

        await Assert.ThrowsAsync<TaskCanceledException>(() => _testFixture.DockerClient.Containers.GetContainerLogsAsync(
            createContainerResponse.ID,
            new ContainerLogsParameters
            {
                ShowStderr = true,
                ShowStdout = true,
                Timestamps = true,
                Follow = true
            },
            containerLogsCts.Token,
            new Progress<string>(m => _testOutputHelper.WriteLine(m))
        ));
    }

    [Fact]
    public async Task GetContainerLogs_Tty_True_Follow_True_Requires_Task_To_Be_Cancelled()
    {
        using var containerLogsCts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = true
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        containerLogsCts.CancelAfter(TimeSpan.FromSeconds(5));

        var containerLogsTask = _testFixture.DockerClient.Containers.GetContainerLogsAsync(
            createContainerResponse.ID,
            new ContainerLogsParameters
            {
                ShowStderr = true,
                ShowStdout = true,
                Timestamps = true,
                Follow = true
            },
            containerLogsCts.Token,
            new Progress<string>(m => _testOutputHelper.WriteLine(m))
        );

        await Assert.ThrowsAsync<TaskCanceledException>(() => containerLogsTask);
    }

    [Fact]
    public async Task GetContainerLogs_Tty_True_Follow_True_ReadsLogs_TaskIsCancelled()
    {
        using var containerLogsCts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        var logList = new List<string>();

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = true
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        containerLogsCts.CancelAfter(TimeSpan.FromSeconds(5));

        var containerLogsTask = _testFixture.DockerClient.Containers.GetContainerLogsAsync(
            createContainerResponse.ID,
            new ContainerLogsParameters
            {
                ShowStderr = true,
                ShowStdout = true,
                Timestamps = true,
                Follow = true
            },
            containerLogsCts.Token,
            new Progress<string>(m => { _testOutputHelper.WriteLine(m); logList.Add(m); })
        );

        await Task.Delay(TimeSpan.FromSeconds(5));

        await _testFixture.DockerClient.Containers.StopContainerAsync(
            createContainerResponse.ID,
            new ContainerStopParameters(),
            _testFixture.Cts.Token
        );


        await Assert.ThrowsAsync<TaskCanceledException>(() => containerLogsTask);
        _testOutputHelper.WriteLine($"Line count: {logList.Count}");

        Assert.NotEmpty(logList);
    }

    [Fact]
    public async Task GetContainerStatsAsync_Tty_False_Stream_False_ReadsStats()
    {
        using var tcs = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);
        var containerStatsList = new List<ContainerStatsResponse>();

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = false
            },
            _testFixture.Cts.Token
        );

        _ = await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        tcs.CancelAfter(TimeSpan.FromSeconds(10));

        await _testFixture.DockerClient.Containers.GetContainerStatsAsync(
            createContainerResponse.ID,
            new ContainerStatsParameters
            {
                Stream = false
            },
            new Progress<ContainerStatsResponse>(m => { _testOutputHelper.WriteLine(m.ID); containerStatsList.Add(m); }),
            tcs.Token
        );

        await Task.Delay(TimeSpan.FromSeconds(10));

        Assert.NotEmpty(containerStatsList);
        Assert.Single(containerStatsList);
        _testOutputHelper.WriteLine($"ConntainerStats count: {containerStatsList.Count}");
    }

    [Fact]
    public async Task GetContainerStatsAsync_Tty_False_StreamStats()
    {
        using var tcs = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);
        using (tcs.Token.Register(() => throw new TimeoutException("GetContainerStatsAsync_Tty_False_StreamStats")))
        {
            var method = MethodBase.GetCurrentMethod();

            _testOutputHelper.WriteLine($"Running test '{method!.Module}' -> '{method!.Name}'");

            var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Image = _testFixture.Image.ID,
                    Name = Guid.NewGuid().ToString(),
                    Tty = false
                },
                _testFixture.Cts.Token
            );

            _ = await _testFixture.DockerClient.Containers.StartContainerAsync(
                createContainerResponse.ID,
                new ContainerStartParameters(),
                _testFixture.Cts.Token
            );

            List<ContainerStatsResponse> containerStatsList = new List<ContainerStatsResponse>();

            using var linkedCts = new CancellationTokenSource();
            linkedCts.CancelAfter(TimeSpan.FromSeconds(5));
            try
            {
                await _testFixture.DockerClient.Containers.GetContainerStatsAsync(
                    createContainerResponse.ID,
                    new ContainerStatsParameters
                    {
                        Stream = true
                    },
                    new Progress<ContainerStatsResponse>(m => { containerStatsList.Add(m); _testOutputHelper.WriteLine(JsonSerializer.Instance.Serialize(m)); }),
                    linkedCts.Token
                );
            }
            catch (OperationCanceledException)
            {
                // this  is expected to  happen on task cancelaltion
            }

            _testOutputHelper.WriteLine($"Container stats count: {containerStatsList.Count}");
            Assert.NotEmpty(containerStatsList);
        }
    }

    [Fact]
    public async Task GetContainerStatsAsync_Tty_True_Stream_False_ReadsStats()
    {
        using var tcs = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);
        var containerStatsList = new List<ContainerStatsResponse>();

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
                Tty = true
            },
            _testFixture.Cts.Token
        );

        _ = await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        tcs.CancelAfter(TimeSpan.FromSeconds(10));

        await _testFixture.DockerClient.Containers.GetContainerStatsAsync(
            createContainerResponse.ID,
            new ContainerStatsParameters
            {
                Stream = false
            },
            new Progress<ContainerStatsResponse>(m => { _testOutputHelper.WriteLine(m.ID); containerStatsList.Add(m); }),
            tcs.Token
        );

        await Task.Delay(TimeSpan.FromSeconds(10));

        Assert.NotEmpty(containerStatsList);
        Assert.Single(containerStatsList);
        _testOutputHelper.WriteLine($"ConntainerStats count: {containerStatsList.Count}");
    }

    [Fact]
    public async Task GetContainerStatsAsync_Tty_True_StreamStats()
    {
        using var tcs = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);

        using (tcs.Token.Register(() => throw new TimeoutException("GetContainerStatsAsync_Tty_True_StreamStats")))
        {
            _testOutputHelper.WriteLine("Running test GetContainerStatsAsync_Tty_True_StreamStats");

            var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Image = _testFixture.Image.ID,
                    Name = Guid.NewGuid().ToString(),
                    Tty = true
                },
                _testFixture.Cts.Token
            );

            _ = await _testFixture.DockerClient.Containers.StartContainerAsync(
                createContainerResponse.ID,
                new ContainerStartParameters(),
                _testFixture.Cts.Token
            );

            List<ContainerStatsResponse> containerStatsList = new List<ContainerStatsResponse>();

            using var linkedTcs = CancellationTokenSource.CreateLinkedTokenSource(tcs.Token);
            linkedTcs.CancelAfter(TimeSpan.FromSeconds(5));

            try
            {
                await _testFixture.DockerClient.Containers.GetContainerStatsAsync(
                    createContainerResponse.ID,
                    new ContainerStatsParameters
                    {
                        Stream = true
                    },
                    new Progress<ContainerStatsResponse>(m => { containerStatsList.Add(m); _testOutputHelper.WriteLine(JsonSerializer.Instance.Serialize(m)); }),
                    linkedTcs.Token
                );
            }
            catch (OperationCanceledException)
            {
                // this  is expected to  happen on task cancelaltion
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
            _testOutputHelper.WriteLine($"Container stats count: {containerStatsList.Count}");
            Assert.NotEmpty(containerStatsList);
        }
    }

    [Fact]
    public async Task KillContainerAsync_ContainerRunning_Succeeds()
    {
        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID
            },
            _testFixture.Cts.Token);

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        var inspectRunningContainerResponse = await _testFixture.DockerClient.Containers.InspectContainerAsync(
            createContainerResponse.ID,
            _testFixture.Cts.Token);

        await _testFixture.DockerClient.Containers.KillContainerAsync(
            createContainerResponse.ID,
            new ContainerKillParameters(),
            _testFixture.Cts.Token);

        var inspectKilledContainerResponse = await _testFixture.DockerClient.Containers.InspectContainerAsync(
            createContainerResponse.ID,
            _testFixture.Cts.Token);

        Assert.True(inspectRunningContainerResponse.State.Running);
        Assert.False(inspectKilledContainerResponse.State.Running);
        Assert.Equal("exited", inspectKilledContainerResponse.State.Status);

        _testOutputHelper.WriteLine("Killed");
        _testOutputHelper.WriteLine(JsonSerializer.Instance.Serialize(inspectKilledContainerResponse));
    }

    [Fact]
    public async Task ListContainersAsync_ContainerExists_Succeeds()
    {
        await _testFixture.DockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString()
            },
            _testFixture.Cts.Token);

        IList<ContainerListResponse> containerList = await _testFixture.DockerClient.Containers.ListContainersAsync(
            new ContainersListParameters
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    ["ancestor"] = new Dictionary<string, bool>
                    {
                        [_testFixture.Image.ID] = true
                    }
                },
                All = true
            },
            _testFixture.Cts.Token
        );

        Assert.NotNull(containerList);
        Assert.NotEmpty(containerList);
    }

    [Fact]
    public async Task ListProcessesAsync_RunningContainer_Succeeds()
    {
        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString()
            },
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        var containerProcessesResponse = await _testFixture.DockerClient.Containers.ListProcessesAsync(
            createContainerResponse.ID,
            new ContainerListProcessesParameters(),
            _testFixture.Cts.Token
        );

        _testOutputHelper.WriteLine($"Title  '{containerProcessesResponse.Titles[0]}' - '{containerProcessesResponse.Titles[1]}' - '{containerProcessesResponse.Titles[2]}' - '{containerProcessesResponse.Titles[3]}'");

        foreach (var processes in containerProcessesResponse.Processes)
        {
            _testOutputHelper.WriteLine($"Process '{processes[0]}' - ''{processes[1]}' - '{processes[2]}' - '{processes[3]}'");
        }

        Assert.NotNull(containerProcessesResponse);
        Assert.NotEmpty(containerProcessesResponse.Processes);
    }

    [Fact]
    public async Task RemoveContainerAsync_ContainerExists_Succeedes()
    {
        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString()
            },
            _testFixture.Cts.Token
        );

        ContainerInspectResponse inspectCreatedContainer = await _testFixture.DockerClient.Containers.InspectContainerAsync(
            createContainerResponse.ID,
            _testFixture.Cts.Token
        );

        await _testFixture.DockerClient.Containers.RemoveContainerAsync(
            createContainerResponse.ID,
            new ContainerRemoveParameters
            {
                Force = true
            },
            _testFixture.Cts.Token
        );

        Task inspectRemovedContainerTask = _testFixture.DockerClient.Containers.InspectContainerAsync(
            createContainerResponse.ID,
            _testFixture.Cts.Token
        );

        Assert.NotNull(inspectCreatedContainer.State);
        await Assert.ThrowsAsync<DockerContainerNotFoundException>(() => inspectRemovedContainerTask);
    }

    [Fact]
    public async Task StartContainerAsync_ContainerExists_Succeeds()
    {
        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString()
            },
            _testFixture.Cts.Token
        );

        var startContainerResult = await _testFixture.DockerClient.Containers.StartContainerAsync(
            createContainerResponse.ID,
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        Assert.True(startContainerResult);
    }

    [Fact]
    public async Task StartContainerAsync_ContainerNotExists_ThrowsException()
    {
        Task startContainerTask = _testFixture.DockerClient.Containers.StartContainerAsync(
            Guid.NewGuid().ToString(),
            new ContainerStartParameters(),
            _testFixture.Cts.Token
        );

        await Assert.ThrowsAsync<DockerContainerNotFoundException>(() => startContainerTask);
    }

    [Fact]
    public async Task WaitContainerAsync_TokenIsCancelled_OperationCancelledException()
    {
        var stopWatch = new Stopwatch();

        using var waitContainerCts = new CancellationTokenSource(delay: TimeSpan.FromMinutes(5));

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(
            new CreateContainerParameters
            {
                Image = _testFixture.Image.ID,
                Name = Guid.NewGuid().ToString(),
            },
            waitContainerCts.Token
        );

        _testOutputHelper.WriteLine($"CreateContainerResponse: '{JsonSerializer.Instance.Serialize(createContainerResponse)}'");

        _ = await _testFixture.DockerClient.Containers.StartContainerAsync(createContainerResponse.ID, new ContainerStartParameters(), waitContainerCts.Token);

        _testOutputHelper.WriteLine("Starting timeout to cancel WaitContainer operation.");

        TimeSpan delay = TimeSpan.FromSeconds(5);

        waitContainerCts.CancelAfter(delay);
        stopWatch.Start();

        // Will wait forever here if cancelation fails.
        var waitContainerTask = _testFixture.DockerClient.Containers.WaitContainerAsync(createContainerResponse.ID, waitContainerCts.Token);

        _ = await Assert.ThrowsAsync<TaskCanceledException>(() => waitContainerTask);

        stopWatch.Stop();

        _testOutputHelper.WriteLine($"WaitContainerTask was cancelled after {stopWatch.ElapsedMilliseconds} ms");
        _testOutputHelper.WriteLine($"WaitContainerAsync: {stopWatch.Elapsed} elapsed");

        // Task should be cancelled when CancelAfter timespan expires
        TimeSpan tolerance = TimeSpan.FromMilliseconds(500);

        Assert.InRange(stopWatch.Elapsed, delay.Subtract(tolerance), delay.Add(tolerance));
        Assert.True(waitContainerTask.IsCanceled);
    }

    [Fact]
    public async Task CreateImageAsync_NonexistantImage_ThrowsDockerImageNotFoundException()
    {
        var parameters = new CreateContainerParameters
        {
            Image = "no-such-image-ytfghbkufhresdhtrjygvb",
        };
        Func<Task> op = async () => await _testFixture.DockerClient.Containers.CreateContainerAsync(parameters);

        await Assert.ThrowsAsync<DockerImageNotFoundException>(op);
    }

    [Fact]
    public async Task MultiplexedStreamWriteAsync_DoesNotThrowAnException()
    {
        // Given
        var createContainerParameters = new CreateContainerParameters();
        createContainerParameters.Image = _testFixture.Image.ID;

        var containerExecCreateParameters = new ContainerExecCreateParameters();
        containerExecCreateParameters.AttachStdout = true;
        containerExecCreateParameters.AttachStderr = true;
        containerExecCreateParameters.AttachStdin = true;
        containerExecCreateParameters.Cmd = new[] { "/bin/sh" };

        var containerExecStartParameters = new ContainerExecStartParameters();

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(createContainerParameters);
        _ = await _testFixture.DockerClient.Containers.StartContainerAsync(createContainerResponse.ID, new ContainerStartParameters());

        // When
        var containerExecCreateResponse = await _testFixture.DockerClient.Exec.ExecCreateContainerAsync(createContainerResponse.ID, containerExecCreateParameters);
        using var stream = await _testFixture.DockerClient.Exec.StartWithConfigContainerExecAsync(containerExecCreateResponse.ID, containerExecStartParameters);
        var buffer = new byte[] { 10 };
        var exception = await Record.ExceptionAsync(() => stream.WriteAsync(buffer, 0, buffer.Length, _testFixture.Cts.Token));

        // Then
        Assert.Null(exception);
    }
}
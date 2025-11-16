namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class ISystemOperationsTests
{
    private readonly TestFixture _testFixture;
    private readonly ITestOutputHelper _testOutputHelper;

    public ISystemOperationsTests(TestFixture testFixture, ITestOutputHelper testOutputHelper)
    {
        _testFixture = testFixture;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Docker_IsRunning()
    {
        var dockerProcess = Process.GetProcesses().FirstOrDefault(process => process.ProcessName.Equals("docker", StringComparison.InvariantCultureIgnoreCase) || process.ProcessName.Equals("dockerd", StringComparison.InvariantCultureIgnoreCase));
        Assert.NotNull(dockerProcess);
    }

    [Fact]
    public async Task GetSystemInfoAsync_Succeeds()
    {
        var info = await _testFixture.DockerClient.System.GetSystemInfoAsync();
        Assert.NotNull(info.Architecture);
    }

    [Fact]
    public async Task GetVersionAsync_Succeeds()
    {
        var version = await _testFixture.DockerClient.System.GetVersionAsync();
        Assert.NotNull(version.APIVersion);
    }

    [Fact]
    public async Task MonitorEventsAsync_EmptyContainersList_CanBeCancelled()
    {
        var progress = new Progress<Message>();

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        await Task.Delay(1);

        await Assert.ThrowsAsync<TaskCanceledException>(() => _testFixture.DockerClient.System.MonitorEventsAsync(new ContainerEventsParameters(), progress, cts.Token));

    }

    [Fact]
    public async Task MonitorEventsAsync_NullParameters_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testFixture.DockerClient.System.MonitorEventsAsync(null, null));
    }

    [Fact]
    public async Task MonitorEventsAsync_NullProgress_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testFixture.DockerClient.System.MonitorEventsAsync(new ContainerEventsParameters(), null));
    }

    [Fact]
    public async Task MonitorEventsAsync_Succeeds()
    {
        var newTag = $"MonitorTests-{Guid.NewGuid().ToString().Substring(1, 10)}";

        var wasProgressCalled = false;

        var progressMessage = new Progress<Message>(m =>
        {
            _testOutputHelper.WriteLine($"MonitorEventsAsync_Succeeds: Message - {m.Action} - {m.Actor.Attributes["name"]} - {m.Type}");
            wasProgressCalled = true;
            Assert.NotNull(m);
        });

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);

        var task = _testFixture.DockerClient.System.MonitorEventsAsync(
            new ContainerEventsParameters(),
            progressMessage,
            cts.Token);

        await _testFixture.DockerClient.Images.TagImageAsync($"{_testFixture.Repository}:{_testFixture.Tag}", new ImageTagParameters { RepositoryName = _testFixture.Repository, Tag = newTag }, _testFixture.Cts.Token);

        await _testFixture.DockerClient.Images.DeleteImageAsync(
            name: $"{_testFixture.Repository}:{newTag}",
            new ImageDeleteParameters
            {
                Force = true
            },
            _testFixture.Cts.Token);

        // Give it some time for output operation to complete before cancelling task
        await Task.Delay(TimeSpan.FromSeconds(1));

        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

        Assert.True(wasProgressCalled);
    }

    [Fact]
    public async Task MonitorEventsAsync_IsCancelled_NoStreamCorruption()
    {
        var stopwatch = new Stopwatch();

        for (int i = 0; i < 20; ++i)
        {
            try
            {
                // (1) Create monitor task
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);

                string newImageTag = Guid.NewGuid().ToString();

                var monitorTask = _testFixture.DockerClient.System.MonitorEventsAsync(
                    new ContainerEventsParameters(),
                    new Progress<Message>(value => _testOutputHelper.WriteLine($"DockerSystemEvent: {JsonSerializer.Instance.Serialize(value)}")),
                    cts.Token);

                // (2) Wait for some time to make sure we get into blocking IO call
                await Task.Delay(100, CancellationToken.None);

                // (3) Invoke another request that will attempt to grab the same buffer
                var listImagesTask1 = _testFixture.DockerClient.Images.TagImageAsync(
                    $"{_testFixture.Repository}:{_testFixture.Tag}",
                    new ImageTagParameters
                    {
                        RepositoryName = _testFixture.Repository,
                        Tag = newImageTag,
                    }, CancellationToken.None);

                // (4) Wait for a short bit again and cancel the monitor task - if we get lucky, we the list images call will grab the same buffer while
                stopwatch.Restart();

                await Task.Delay(100, CancellationToken.None);

                _testOutputHelper.WriteLine($"Waited for {stopwatch.Elapsed.TotalMilliseconds} ms");

                await cts.CancelAsync();

                await listImagesTask1;

                await _testFixture.DockerClient.Images.TagImageAsync(
                    $"{_testFixture.Repository}:{_testFixture.Tag}",
                    new ImageTagParameters
                    {
                        RepositoryName = _testFixture.Repository,
                        Tag = newImageTag,
                    }, CancellationToken.None);

                await monitorTask;
            }
            catch (OperationCanceledException)
            {
                // Exceptions other than this causes test to fail
            }
        }
    }

    [Fact]
    public async Task MonitorEventsFiltered_Succeeds()
    {
        string newTag = $"MonitorTests-{Guid.NewGuid().ToString().Substring(1, 10)}";
        string newImageRepositoryName = Guid.NewGuid().ToString();

        await _testFixture.DockerClient.Images.TagImageAsync(
            $"{_testFixture.Repository}:{_testFixture.Tag}",
            new ImageTagParameters
            {
                RepositoryName = newImageRepositoryName,
                Tag = newTag
            },
            _testFixture.Cts.Token
        );

        ImageInspectResponse image = await _testFixture.DockerClient.Images.InspectImageAsync(
            $"{newImageRepositoryName}:{newTag}",
            _testFixture.Cts.Token
        );

        var progressCalledCounter = 0;

        var eventsParams = new ContainerEventsParameters
        {
            Filters = new Dictionary<string, IDictionary<string, bool>>
            {
                {
                    "event", new Dictionary<string, bool>
                    {
                        {
                            "tag", true
                        },
                        {
                            "untag", true
                        }
                    }
                },
                {
                    "type", new Dictionary<string, bool>
                    {
                        {
                            "image", true
                        }
                    }
                },
                {
                    "image", new Dictionary<string, bool>
                    {
                        {
                            image.ID, true
                        }
                    }
                }
            }
        };

        var progress = new Progress<Message>(m =>
        {
            Interlocked.Increment(ref progressCalledCounter);
            Assert.True(m.Action == "tag" || m.Action == "untag");
            _testOutputHelper.WriteLine($"MonitorEventsFiltered_Succeeds: Message - {m.Action} - {m.Actor.Attributes["name"]} - {m.Type}");
        });

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);
        var task = Task.Run(() => _testFixture.DockerClient.System.MonitorEventsAsync(eventsParams, progress, cts.Token));

        await _testFixture.DockerClient.Images.TagImageAsync($"{_testFixture.Repository}:{_testFixture.Tag}", new ImageTagParameters { RepositoryName = _testFixture.Repository, Tag = newTag });
        await _testFixture.DockerClient.Images.DeleteImageAsync($"{_testFixture.Repository}:{newTag}", new ImageDeleteParameters());

        var createContainerResponse = await _testFixture.DockerClient.Containers.CreateContainerAsync(new CreateContainerParameters { Image = $"{_testFixture.Repository}:{_testFixture.Tag}", Entrypoint = CommonCommands.SleepInfinity });
        await _testFixture.DockerClient.Containers.RemoveContainerAsync(createContainerResponse.ID, new ContainerRemoveParameters(), cts.Token);

        await Task.Delay(TimeSpan.FromSeconds(1));
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

        Assert.Equal(2, progressCalledCounter);
        Assert.True(task.IsCanceled);
    }

    [Fact]
    public async Task PingAsync_Succeeds()
    {
        await _testFixture.DockerClient.System.PingAsync();
    }
}
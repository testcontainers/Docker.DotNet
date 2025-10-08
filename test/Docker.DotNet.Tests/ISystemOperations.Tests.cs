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

    public static IEnumerable<object[]> GetDockerClientTypes() =>
        TestFixture.GetDockerClientTypes();

    [Fact]
    public void Docker_IsRunning()
    {
        var processNames = Process.GetProcesses().Select(Process => Process.ProcessName);
        var dockerProcess = processNames.FirstOrDefault(
            name => name.Equals("docker", StringComparison.InvariantCultureIgnoreCase)
            || name.Equals("com.docker.service", StringComparison.InvariantCultureIgnoreCase)
            || name.Equals("dockerd", StringComparison.InvariantCultureIgnoreCase));
        Assert.NotNull(dockerProcess);
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task GetSystemInfoAsync_Succeeds(TestClientsEnum clientType)
    {
        var info = await _testFixture.DockerClients[clientType].System.GetSystemInfoAsync();
        Assert.NotNull(info.Architecture);
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task GetVersionAsync_Succeeds(TestClientsEnum clientType)
    {
        var version = await _testFixture.DockerClients[clientType].System.GetVersionAsync();
        Assert.NotNull(version.APIVersion);
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task MonitorEventsAsync_EmptyContainersList_CanBeCancelled(TestClientsEnum clientType)
    {
        var progress = new Progress<Message>();

        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        await Task.Delay(1);

        await Assert.ThrowsAsync<TaskCanceledException>(() => _testFixture.DockerClients[clientType].System.MonitorEventsAsync(new ContainerEventsParameters(), progress, cts.Token));

    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task MonitorEventsAsync_NullParameters_Throws(TestClientsEnum clientType)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testFixture.DockerClients[clientType].System.MonitorEventsAsync(null, null));
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task MonitorEventsAsync_NullProgress_Throws(TestClientsEnum clientType)
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _testFixture.DockerClients[clientType].System.MonitorEventsAsync(new ContainerEventsParameters(), null));
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task MonitorEventsAsync_Succeeds(TestClientsEnum clientType)
    {
        var newTag = $"MonitorTests-{Guid.NewGuid().ToString().Substring(1, 10)}";

        var wasProgressCalled = false;

        var progressMessage = new Progress<Message>(m =>
        {
            _testOutputHelper.WriteLine($"MonitorEventsAsync_Succeeds: Message - {m.Action} - {m.Status} {m.From} - {m.Type}");
            wasProgressCalled = true;
            Assert.NotNull(m);
        });

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);

        var task = _testFixture.DockerClients[clientType].System.MonitorEventsAsync(
            new ContainerEventsParameters(),
            progressMessage,
            cts.Token);

        await _testFixture.DockerClients[clientType].Images.TagImageAsync($"{_testFixture.Repository}:{_testFixture.Tag}", new ImageTagParameters { RepositoryName = _testFixture.Repository, Tag = newTag }, _testFixture.Cts.Token);

        await _testFixture.DockerClients[clientType].Images.DeleteImageAsync(
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

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task MonitorEventsAsync_IsCancelled_NoStreamCorruption(TestClientsEnum clientType)
    {
        var rand = new Random();
        var sw = new Stopwatch();

        for (int i = 0; i < 20; ++i)
        {
            try
            {
                // (1) Create monitor task
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);

                string newImageTag = Guid.NewGuid().ToString();

                var monitorTask = _testFixture.DockerClients[clientType].System.MonitorEventsAsync(
                    new ContainerEventsParameters(),
                    new Progress<Message>(value => _testOutputHelper.WriteLine($"DockerSystemEvent: {JsonSerializer.Instance.Serialize(value)}")),
                    cts.Token);

                // (2) Wait for some time to make sure we get into blocking IO call
                await Task.Delay(100, CancellationToken.None);

                // (3) Invoke another request that will attempt to grab the same buffer
                var listImagesTask1 = _testFixture.DockerClients[clientType].Images.TagImageAsync(
                    $"{_testFixture.Repository}:{_testFixture.Tag}",
                    new ImageTagParameters
                    {
                        RepositoryName = _testFixture.Repository,
                        Tag = newImageTag,
                    }, CancellationToken.None);

                // (4) Wait for a short bit again and cancel the monitor task - if we get lucky, we the list images call will grab the same buffer while
                sw.Restart();
                var iterations = rand.Next(15000000);

                for (var j = 0; j < iterations; j++)
                {
                    // noop
                }

                _testOutputHelper.WriteLine($"Waited for {sw.Elapsed.TotalMilliseconds} ms");

                await cts.CancelAsync();

                await listImagesTask1;

                await _testFixture.DockerClients[clientType].Images.TagImageAsync(
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

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task MonitorEventsFiltered_Succeeds(TestClientsEnum clientType)
    {
        string newTag = $"MonitorTests-{Guid.NewGuid().ToString().Substring(1, 10)}";
        string newImageRepositoryName = Guid.NewGuid().ToString();

        await _testFixture.DockerClients[clientType].Images.TagImageAsync(
            $"{_testFixture.Repository}:{_testFixture.Tag}",
            new ImageTagParameters
            {
                RepositoryName = newImageRepositoryName,
                Tag = newTag
            },
            _testFixture.Cts.Token
        );

        ImageInspectResponse image = await _testFixture.DockerClients[clientType].Images.InspectImageAsync(
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
            Assert.True(m.Status == "tag" || m.Status == "untag");
            _testOutputHelper.WriteLine($"MonitorEventsFiltered_Succeeds: Message received: {m.Action} - {m.Status} {m.From} - {m.Type}");
        });

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_testFixture.Cts.Token);
        var task = Task.Run(() => _testFixture.DockerClients[clientType].System.MonitorEventsAsync(eventsParams, progress, cts.Token));

        await Task.Delay(TimeSpan.FromSeconds(1));

        await _testFixture.DockerClients[clientType].Images.TagImageAsync($"{_testFixture.Repository}:{_testFixture.Tag}", new ImageTagParameters { RepositoryName = _testFixture.Repository, Tag = newTag });
        await _testFixture.DockerClients[clientType].Images.DeleteImageAsync($"{_testFixture.Repository}:{newTag}", new ImageDeleteParameters());

        var createContainerResponse = await _testFixture.DockerClients[clientType].Containers.CreateContainerAsync(new CreateContainerParameters { Image = $"{_testFixture.Repository}:{_testFixture.Tag}", Entrypoint = CommonCommands.SleepInfinity });
        await _testFixture.DockerClients[clientType].Containers.RemoveContainerAsync(createContainerResponse.ID, new ContainerRemoveParameters(), cts.Token);

        await Task.Delay(TimeSpan.FromSeconds(1));
        await cts.CancelAsync();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);

        Assert.Equal(2, progressCalledCounter);
        Assert.True(task.IsCanceled);
    }

    [Theory]
    [MemberData(nameof(GetDockerClientTypes))]
    public async Task PingAsync_Succeeds(TestClientsEnum clientType)
    {
        await _testFixture.DockerClients[clientType].System.PingAsync();
    }
}
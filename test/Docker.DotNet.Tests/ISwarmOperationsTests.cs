namespace Docker.DotNet.Tests;

[Collection(nameof(TestCollection))]
public class ISwarmOperationsTests
{
    private readonly CancellationTokenSource _cts;

    private readonly DockerClient _dockerClient;
    private readonly string _imageId;

    public ISwarmOperationsTests(TestFixture testFixture)
    {
        // Do not wait forever in case it gets stuck
        _cts = CancellationTokenSource.CreateLinkedTokenSource(testFixture.Cts.Token);
        _cts.CancelAfter(TimeSpan.FromMinutes(5));
        _cts.Token.Register(() => throw new TimeoutException("SwarmOperationTests timeout"));

        _dockerClient = testFixture.DockerClient;
        _imageId = testFixture.Image.ID;
    }

    [Fact]
    public async Task GetFilteredServicesByName_Succeeds()
    {
        var serviceName = $"service1-{Guid.NewGuid().ToString().Substring(1, 10)}";

        var firstServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = serviceName,
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var secondServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service2-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var thirdServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service3-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var services = await _dockerClient.Swarm.ListServicesAsync(new ServiceListParameters
        {
            Filters = new Dictionary<string, IDictionary<string, bool>>
            {
                ["name"] = new Dictionary<string, bool>
                {
                    [serviceName] = true
                }
            }
        }, CancellationToken.None);

        Assert.Single(services);

        await _dockerClient.Swarm.RemoveServiceAsync(firstServiceId);
        await _dockerClient.Swarm.RemoveServiceAsync(secondServiceId);
        await _dockerClient.Swarm.RemoveServiceAsync(thirdServiceId);
    }

    [Fact]
    public async Task GetFilteredServicesById_Succeeds()
    {
        var firstServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service1-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var secondServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service2-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var thirdServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service3-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var services = await _dockerClient.Swarm.ListServicesAsync(new ServiceListParameters
        {
            Filters = new Dictionary<string, IDictionary<string, bool>>
            {
                ["id"] = new Dictionary<string, bool>
                {
                    [firstServiceId] = true
                }
            }
        }, CancellationToken.None);

        Assert.Single(services);

        await _dockerClient.Swarm.RemoveServiceAsync(firstServiceId);
        await _dockerClient.Swarm.RemoveServiceAsync(secondServiceId);
        await _dockerClient.Swarm.RemoveServiceAsync(thirdServiceId);
    }

    [Fact]
    public async Task GetServices_Succeeds()
    {
        var initialServiceCount = (await _dockerClient.Swarm.ListServicesAsync(cancellationToken: CancellationToken.None)).Count();

        var firstServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service1-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var secondServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service2-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var thirdServiceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = $"service3-{Guid.NewGuid().ToString().Substring(1, 10)}",
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var services = await _dockerClient.Swarm.ListServicesAsync(cancellationToken: CancellationToken.None);

        Assert.True(services.Count() > initialServiceCount);

        await _dockerClient.Swarm.RemoveServiceAsync(firstServiceId);
        await _dockerClient.Swarm.RemoveServiceAsync(secondServiceId);
        await _dockerClient.Swarm.RemoveServiceAsync(thirdServiceId);
    }

    [Fact]
    public async Task GetServiceLogs_Succeeds()
    {
        var cts = new CancellationTokenSource();
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cts.Token);

        var serviceName = $"service-withLogs-{Guid.NewGuid().ToString().Substring(1, 10)}";
        var serviceId = (await _dockerClient.Swarm.CreateServiceAsync(new ServiceCreateParameters
        {
            Service = new ServiceSpec
            {
                Name = serviceName,
                TaskTemplate = new TaskSpec { ContainerSpec = new ContainerSpec { Image = _imageId } }
            }
        })).ID;

        var stream = await _dockerClient.Swarm.GetServiceLogsAsync(serviceName, false, new ServiceLogsParameters
        {
            Follow = true,
            ShowStdout = true,
            ShowStderr = true
        });

        var maxRetries = 3;
        var currentRetry = 0;
        var delayBetweenRetries = TimeSpan.FromSeconds(5);
        List<string> logLines = null;

        while (currentRetry < maxRetries && !linkedCts.IsCancellationRequested)
        {
            logLines = new List<string>();
            var delay = TimeSpan.FromSeconds(10);
            cts.CancelAfter(delay);

            var cancelRequested = false; // Add a flag to indicate cancellation

            while (!linkedCts.IsCancellationRequested && !cancelRequested)
            {
                var line = new List<byte>();
                var buffer = new byte[4096];

                try
                {
                    while (true)
                    {
                        var res = await stream.ReadOutputAsync(buffer, 0, buffer.Length, linkedCts.Token);

                        if (res.Count == 0)
                        {
                            continue;
                        }

                        var newlineIndex = Array.IndexOf(buffer, (byte)'\n', 0, res.Count);

                        if (newlineIndex != -1)
                        {
                            line.AddRange(buffer.Take(newlineIndex));
                            break;
                        }
                        else
                        {
                            line.AddRange(buffer.Take(res.Count));
                        }
                    }

                    logLines.Add(Encoding.UTF8.GetString(line.ToArray()));
                }
                catch (OperationCanceledException)
                {
                    cancelRequested = true; // Set the flag when cancellation is requested

                    // Reset the CancellationTokenSource for the next attempt
                    cts = new CancellationTokenSource();
                    linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, cts.Token);
                    cts.CancelAfter(delay);
                }
            }

            if (logLines.Any() && logLines.First().Contains("[INF]"))
            {
                break;
            }
            else
            {
                currentRetry++;
                if (currentRetry < maxRetries)
                {
                    await Task.Delay(delayBetweenRetries);
                }
            }
        }

        Assert.NotNull(logLines);
        Assert.NotEmpty(logLines);
        Assert.Contains("[INF]", logLines.First());

        await _dockerClient.Swarm.RemoveServiceAsync(serviceId);
    }
}
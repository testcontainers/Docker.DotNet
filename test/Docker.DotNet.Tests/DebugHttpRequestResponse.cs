namespace Docker.DotNet.Tests;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

public sealed class DebugHttpRequestResponse(ITestOutputHelper testOutputHelper) : ILogger
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

    [Fact]
    public async Task Run()
    {
        const string image = "postgres:alpine";
        // TODO: Set the Docker host address according to your environment.
        using var dockerClientConfiguration = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"));
        using var dockerClient = dockerClientConfiguration.CreateClient(logger: this);
        // await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters { FromImage = image }, new AuthConfig(), new Progress<JSONMessage>());
        var createContainerResponse = await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters { Image = image });
        var inspectContainerResponse = await dockerClient.Containers.InspectContainerAsync(createContainerResponse.ID);
        _ = inspectContainerResponse;
    }

    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        ILogger logger = this;

        if (!logger.IsEnabled(logLevel))
        {
            return;
        }

        var message = exception == null ? formatter.Invoke(state, null) : string.Join(Environment.NewLine, formatter.Invoke(state, exception), exception);
        _testOutputHelper.WriteLine("[Docker.DotNet {0:hh\\:mm\\:ss\\.ff}] {1}", _stopwatch.Elapsed, message);
    }

    bool ILogger.IsEnabled(LogLevel logLevel)
    {
        return logLevel >= LogLevel.Debug;
    }

    IDisposable ILogger.BeginScope<TState>(TState state)
    {
        return new Disposable();
    }

    private sealed class Disposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}
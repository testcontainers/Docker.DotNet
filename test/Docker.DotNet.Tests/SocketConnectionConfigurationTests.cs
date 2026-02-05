namespace Docker.DotNet.Tests;

using System.Net.Sockets;
using Docker.DotNet.Handler.Abstractions;

public class SocketConnectionConfigurationTests
{
    [Fact]
    public void Default_HasExpectedValues()
    {
        var config = SocketConnectionConfiguration.Default;

        Assert.True(config.KeepAlive);
        Assert.True(config.NoDelay);
        Assert.Null(config.SendBufferSize);
        Assert.Null(config.ReceiveBufferSize);
    }

    [Fact]
    public void Default_IsSingleton()
    {
        Assert.Same(SocketConnectionConfiguration.Default, SocketConnectionConfiguration.Default);
    }

    [Fact]
    public void Apply_SetsTcpSocketOptions()
    {
        var config = new SocketConnectionConfiguration
        {
            KeepAlive = true,
            NoDelay = false,
        };

        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        config.Apply(socket);

        Assert.False(socket.NoDelay);
    }

    [Fact]
    public void Apply_UnixSocket_DoesNotThrow()
    {
        // TCP-specific options (NoDelay, KeepAliveTime, etc.) must be skipped
        // for Unix domain sockets, which use ProtocolType.Unspecified.
        var config = new SocketConnectionConfiguration
        {
            KeepAlive = true,
            NoDelay = true,
        };

        using var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
        var exception = Record.Exception(() => config.Apply(socket));

        Assert.Null(exception);
    }

    [Fact]
    public void Apply_ThrowsOnNullSocket()
    {
        var config = new SocketConnectionConfiguration();

        Assert.Throws<ArgumentNullException>(() => config.Apply(null));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void KeepAliveTime_RejectsInvalidValues(int value)
    {
        var config = new SocketConnectionConfiguration();

        Assert.Throws<ArgumentOutOfRangeException>(() => config.KeepAliveTime = value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void KeepAliveInterval_RejectsInvalidValues(int value)
    {
        var config = new SocketConnectionConfiguration();

        Assert.Throws<ArgumentOutOfRangeException>(() => config.KeepAliveInterval = value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void KeepAliveRetryCount_RejectsInvalidValues(int value)
    {
        var config = new SocketConnectionConfiguration();

        Assert.Throws<ArgumentOutOfRangeException>(() => config.KeepAliveRetryCount = value);
    }
}

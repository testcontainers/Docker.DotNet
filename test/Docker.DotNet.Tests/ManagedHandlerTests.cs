namespace Docker.DotNet.Tests;

using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.Net.Http.Client;
using Xunit;

/// <summary>
/// Tests for ManagedHandler modernization features.
/// </summary>
public class ManagedHandlerTests
{
    #region Connection Timeout Tests

    [Fact]
    public void ManagedHandler_ConnectTimeout_HasDefaultValue()
    {
        // Arrange & Act
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(30), handler.ConnectTimeout);
    }

    [Fact]
    public void ManagedHandler_ConnectTimeout_CanBeSet()
    {
        // Arrange
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Act
        handler.ConnectTimeout = TimeSpan.FromSeconds(60);

        // Assert
        Assert.Equal(TimeSpan.FromSeconds(60), handler.ConnectTimeout);
    }

    [Fact]
    public void ManagedHandler_ConnectTimeout_CanBeDisabled()
    {
        // Arrange
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Act
        handler.ConnectTimeout = Timeout.InfiniteTimeSpan;

        // Assert
        Assert.Equal(Timeout.InfiniteTimeSpan, handler.ConnectTimeout);
    }

    #endregion

    #region Phase 4: Happy Eyeballs Tests

#if NET6_0_OR_GREATER
    [Fact]
    public void SocketConnectionConfiguration_HappyEyeballs_DefaultEnabled()
    {
        // Arrange & Act
        var config = new SocketConnectionConfiguration();

        // Assert
        Assert.True(config.EnableHappyEyeballs);
        Assert.Equal(TimeSpan.FromMilliseconds(250), config.HappyEyeballsDelay);
    }

    [Fact]
    public void SocketConnectionConfiguration_HappyEyeballs_CanBeDisabled()
    {
        // Arrange
        var config = new SocketConnectionConfiguration();

        // Act
        config.EnableHappyEyeballs = false;

        // Assert
        Assert.False(config.EnableHappyEyeballs);
    }

    [Fact]
    public void SocketConnectionConfiguration_HappyEyeballsDelay_CanBeCustomized()
    {
        // Arrange
        var config = new SocketConnectionConfiguration();

        // Act
        config.HappyEyeballsDelay = TimeSpan.FromMilliseconds(500);

        // Assert
        Assert.Equal(TimeSpan.FromMilliseconds(500), config.HappyEyeballsDelay);
    }
#endif

    #endregion

    #region Phase 6: Modern Proxy Resolution Tests

    [Fact]
    public void ManagedHandler_Proxy_CanBeSet()
    {
        // Arrange
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);
        var proxy = new System.Net.WebProxy("http://proxy.example.com:8080");

        // Act
        handler.Proxy = proxy;

        // Assert
        Assert.Equal(proxy, handler.Proxy);
    }

    [Fact]
    public void ManagedHandler_UseProxy_DefaultTrue()
    {
        // Arrange & Act
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Assert
        Assert.True(handler.UseProxy);
    }

    #endregion

    #region Phase 7: Line Reading Tests

#if NET6_0_OR_GREATER
    [Fact]
    public async Task BufferedReadStream_ReadLineAsync_ReadsLine()
    {
        // Arrange
        var testData = "HTTP/1.1 200 OK\r\nContent-Length: 0\r\n\r\n";
        var mockStream = new MemoryStream(Encoding.ASCII.GetBytes(testData));
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var bufferedStream = new BufferedReadStream(mockStream, null, logger);

        // Act
        var line = await bufferedStream.ReadLineAsync(CancellationToken.None);

        // Assert
        Assert.Equal("HTTP/1.1 200 OK", line);
    }
#endif

    #endregion

    #region Integration Tests

    [Fact]
    public void ManagedHandler_FullConfiguration_AllPropertiesAccessible()
    {
        // Arrange & Act
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Assert - All properties should be accessible
        Assert.NotNull(handler.Proxy);
        Assert.True(handler.UseProxy);
        Assert.Equal(20, handler.MaxAutomaticRedirects);
        Assert.Equal(RedirectMode.NoDowngrade, handler.RedirectMode);
        Assert.Null(handler.ServerCertificateValidationCallback);
        Assert.NotNull(handler.ClientCertificates);
        Assert.Equal(TimeSpan.FromSeconds(30), handler.ConnectTimeout);
    }

    [Fact]
    public void SocketConnectionConfiguration_FullConfiguration_AllPropertiesAccessible()
    {
        // Arrange & Act
        var config = new SocketConnectionConfiguration();

        // Assert - All properties should be accessible
        Assert.True(config.KeepAlive);
        Assert.Equal(30, config.KeepAliveTime);
        Assert.Equal(10, config.KeepAliveInterval);
        Assert.Equal(3, config.KeepAliveRetryCount);
        Assert.True(config.NoDelay);
        Assert.Null(config.SendBufferSize);
        Assert.Null(config.ReceiveBufferSize);
#if NET6_0_OR_GREATER
        Assert.True(config.EnableHappyEyeballs);
        Assert.Equal(TimeSpan.FromMilliseconds(250), config.HappyEyeballsDelay);
#endif
    }

    #endregion
}

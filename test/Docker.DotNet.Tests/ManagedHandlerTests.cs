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
    #region Phase 1: Memory-Efficient APIs & IAsyncDisposable Tests

    [Fact]
    public async Task BufferedReadStream_ImplementsIAsyncDisposable()
    {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        // Arrange
        var mockStream = new MemoryStream(Encoding.ASCII.GetBytes("Hello, World!"));
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        var bufferedStream = new BufferedReadStream(mockStream, null, logger);

        // Act & Assert
        Assert.True(bufferedStream is IAsyncDisposable);
        await ((IAsyncDisposable)bufferedStream).DisposeAsync();
#else
        await Task.CompletedTask;
#endif
    }

#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
    [Fact]
    public async Task BufferedReadStream_ReadAsyncMemory_ReadsFromBuffer()
    {
        // Arrange
        var testData = "Hello, World!"u8.ToArray();
        var mockStream = new MemoryStream(testData);
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        var bufferedStream = new BufferedReadStream(mockStream, null, logger);

        // Act
        var buffer = new byte[5];
        var bytesRead = await bufferedStream.ReadAsync(buffer.AsMemory());

        // Assert
        Assert.Equal(5, bytesRead);
        Assert.Equal("Hello", Encoding.ASCII.GetString(buffer));

        await bufferedStream.DisposeAsync();
    }

    [Fact]
    public async Task BufferedReadStream_WriteAsyncMemory_WritesToInner()
    {
        // Arrange
        var outputStream = new MemoryStream();
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        var bufferedStream = new BufferedReadStream(outputStream, null, logger);

        // Act
        var data = "Test Data"u8.ToArray();
        await bufferedStream.WriteAsync(data.AsMemory());

        // Assert
        outputStream.Position = 0;
        var result = new byte[data.Length];
        await outputStream.ReadAsync(result.AsMemory());
        Assert.Equal("Test Data", Encoding.ASCII.GetString(result));

        await bufferedStream.DisposeAsync();
    }
#endif

    #endregion

    #region Phase 2: Connection Timeout Tests

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

    #region Phase 3: Retry Policy Tests

    [Fact]
    public void RetryPolicy_Default_HasSensibleDefaults()
    {
        // Arrange & Act
        var policy = RetryPolicy.Default;

        // Assert
        Assert.Equal(3, policy.MaxRetries);
        Assert.Equal(TimeSpan.FromMilliseconds(100), policy.InitialDelay);
        Assert.Equal(TimeSpan.FromSeconds(10), policy.MaxDelay);
        Assert.Equal(0.2, policy.JitterFactor);
    }

    [Fact]
    public void RetryPolicy_NoRetry_DisablesRetries()
    {
        // Arrange & Act
        var policy = RetryPolicy.NoRetry;

        // Assert
        Assert.Equal(0, policy.MaxRetries);
    }

    [Theory]
    [InlineData(SocketError.ConnectionRefused, true)]
    [InlineData(SocketError.ConnectionReset, true)]
    [InlineData(SocketError.HostUnreachable, true)]
    [InlineData(SocketError.NetworkUnreachable, true)]
    [InlineData(SocketError.TimedOut, true)]
    [InlineData(SocketError.TryAgain, true)]
    [InlineData(SocketError.AccessDenied, false)]
    [InlineData(SocketError.AddressAlreadyInUse, false)]
    public void RetryPolicy_IsTransientError_ClassifiesSocketErrors(SocketError errorCode, bool expected)
    {
        // Arrange
        var policy = new RetryPolicy();
        var exception = new SocketException((int)errorCode);

        // Act
        var isTransient = policy.IsTransientError(exception);

        // Assert
        Assert.Equal(expected, isTransient);
    }

    [Theory]
    [InlineData(1, 100)]   // First retry: 100ms
    [InlineData(2, 200)]   // Second retry: 200ms
    [InlineData(3, 400)]   // Third retry: 400ms
    [InlineData(10, 10000)] // Should cap at MaxDelay (10s)
    public void RetryPolicy_CalculateDelay_UsesExponentialBackoff(int attempt, double expectedBaseDelayMs)
    {
        // Arrange
        var policy = new RetryPolicy { JitterFactor = 0 }; // Disable jitter for deterministic testing

        // Act
        var delay = policy.CalculateDelay(attempt);

        // Assert
        Assert.Equal(expectedBaseDelayMs, delay.TotalMilliseconds);
    }

    [Fact]
    public async Task RetryPolicy_ExecuteAsync_RetriesOnTransientError()
    {
        // Arrange
        var policy = new RetryPolicy { MaxRetries = 2, InitialDelay = TimeSpan.FromMilliseconds(1) };
        var attempts = 0;

        // Act
        var result = await policy.ExecuteAsync(async ct =>
        {
            attempts++;
            if (attempts < 3)
            {
                throw new SocketException((int)SocketError.ConnectionRefused);
            }
            return 42;
        }, CancellationToken.None);

        // Assert
        Assert.Equal(3, attempts);
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task RetryPolicy_ExecuteAsync_ThrowsOnExhaustion()
    {
        // Arrange
        var policy = new RetryPolicy { MaxRetries = 2, InitialDelay = TimeSpan.FromMilliseconds(1) };
        var attempts = 0;

        // Act & Assert
        var ex = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            await policy.ExecuteAsync<int>(async ct =>
            {
                attempts++;
                throw new SocketException((int)SocketError.ConnectionRefused);
            }, CancellationToken.None);
        });

        // Should have tried 3 times (initial + 2 retries)
        Assert.Equal(3, attempts);
        Assert.True(ex is AggregateException || ex is SocketException);
    }

    [Fact]
    public void ManagedHandler_RetryPolicy_HasDefault()
    {
        // Arrange & Act
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Assert
        Assert.NotNull(handler.RetryPolicy);
        Assert.Equal(RetryPolicy.Default.MaxRetries, handler.RetryPolicy.MaxRetries);
    }

    [Fact]
    public void ManagedHandler_RetryPolicy_CanBeDisabled()
    {
        // Arrange
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Act
        handler.RetryPolicy = RetryPolicy.NoRetry;

        // Assert
        Assert.Equal(0, handler.RetryPolicy.MaxRetries);
    }

    #endregion

    #region Phase 4: Happy Eyeballs Tests

#if NET6_0_OR_GREATER
    [Fact]
    public void SocketConfiguration_HappyEyeballs_DefaultEnabled()
    {
        // Arrange & Act
        var config = new SocketConfiguration();

        // Assert
        Assert.True(config.EnableHappyEyeballs);
        Assert.Equal(TimeSpan.FromMilliseconds(250), config.HappyEyeballsDelay);
    }

    [Fact]
    public void SocketConfiguration_HappyEyeballs_CanBeDisabled()
    {
        // Arrange
        var config = new SocketConfiguration();

        // Act
        config.EnableHappyEyeballs = false;

        // Assert
        Assert.False(config.EnableHappyEyeballs);
    }

    [Fact]
    public void SocketConfiguration_HappyEyeballsDelay_CanBeCustomized()
    {
        // Arrange
        var config = new SocketConfiguration();

        // Act
        config.HappyEyeballsDelay = TimeSpan.FromMilliseconds(500);

        // Assert
        Assert.Equal(TimeSpan.FromMilliseconds(500), config.HappyEyeballsDelay);
    }
#endif

    #endregion

    #region Phase 5: Connection Pooling Tests

#if NET5_0_OR_GREATER
    [Fact]
    public void ManagedHandler_ConnectionPooling_DefaultEnabled()
    {
        // Arrange & Act
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Assert
        Assert.True(handler.EnableConnectionPooling);
    }

    [Fact]
    public void ManagedHandler_ConnectionPooling_CanBeDisabled()
    {
        // Arrange
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var handler = new ManagedHandler(logger);

        // Act
        handler.EnableConnectionPooling = false;

        // Assert
        Assert.False(handler.EnableConnectionPooling);
    }

    [Fact]
    public void ConnectionPool_Default_HasSensibleDefaults()
    {
        // Arrange & Act
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        var pool = new ConnectionPool(logger);

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(2), pool.IdleTimeout);
        Assert.Equal(TimeSpan.FromMinutes(10), pool.ConnectionLifetime);
        Assert.Equal(10, pool.MaxConnectionsPerHost);

        pool.Dispose();
    }

    [Fact]
    public void ConnectionPool_TryGetConnection_ReturnsFalseWhenEmpty()
    {
        // Arrange
        var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<ManagedHandlerTests>();
        using var pool = new ConnectionPool(logger);

        // Act
        var result = pool.TryGetConnection("localhost", 2375, false, out var connection);

        // Assert
        Assert.False(result);
        Assert.Null(connection);
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
        await using var bufferedStream = new BufferedReadStream(mockStream, null, logger);

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
        Assert.NotNull(handler.RetryPolicy);
#if NET5_0_OR_GREATER
        Assert.True(handler.EnableConnectionPooling);
#endif
    }

    [Fact]
    public void SocketConfiguration_FullConfiguration_AllPropertiesAccessible()
    {
        // Arrange & Act
        var config = new SocketConfiguration();

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

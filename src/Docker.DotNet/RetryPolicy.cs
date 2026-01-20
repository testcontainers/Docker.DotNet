namespace Docker.DotNet;

/// <summary>
/// Configures retry behavior for transient connection failures.
/// </summary>
public sealed class RetryPolicy
{
    private static readonly Random Jitter = new Random();

    /// <summary>
    /// Gets the default retry policy with sensible defaults.
    /// </summary>
    public static RetryPolicy Default { get; } = new RetryPolicy();

    /// <summary>
    /// Gets a retry policy that disables retries.
    /// </summary>
    public static RetryPolicy NoRetry { get; } = new RetryPolicy { MaxRetries = 0 };

    /// <summary>
    /// Gets or sets the maximum number of retry attempts. Default is 3.
    /// Set to 0 to disable retries.
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets or sets the initial delay before the first retry. Default is 100ms.
    /// </summary>
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Gets or sets the maximum delay between retries. Default is 10 seconds.
    /// </summary>
    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Gets or sets the jitter factor to add randomness to delays. Default is 0.2 (20%).
    /// This helps prevent thundering herd problems when multiple clients retry simultaneously.
    /// </summary>
    public double JitterFactor { get; set; } = 0.2;

    /// <summary>
    /// Gets or sets the backoff multiplier for exponential backoff. Default is 2.0.
    /// </summary>
    public double BackoffMultiplier { get; set; } = 2.0;

    /// <summary>
    /// Determines if the specified socket exception represents a transient error that can be retried.
    /// </summary>
    /// <param name="exception">The socket exception to evaluate.</param>
    /// <returns>True if the error is transient and the operation can be retried; otherwise, false.</returns>
    public bool IsTransientError(SocketException exception)
    {
        if (exception == null)
        {
            return false;
        }

        // Transient socket errors that are safe to retry
        return exception.SocketErrorCode switch
        {
            // Connection refused - server might not be ready yet
            SocketError.ConnectionRefused => true,

            // Connection reset - connection was forcibly closed
            SocketError.ConnectionReset => true,

            // Host unreachable - temporary network issue
            SocketError.HostUnreachable => true,

            // Network unreachable - temporary network issue
            SocketError.NetworkUnreachable => true,

            // Timed out - might succeed on retry
            SocketError.TimedOut => true,

            // Try again - explicit retry suggestion
            SocketError.TryAgain => true,

            // Host not found - DNS might be propagating
            SocketError.HostNotFound => true,

            // No buffer space available - temporary resource constraint
            SocketError.NoBufferSpaceAvailable => true,

            // Too many open sockets - temporary resource constraint
            SocketError.TooManyOpenSockets => true,

            // Connection aborted - connection was aborted by the network
            SocketError.ConnectionAborted => true,

            // All other errors are not considered transient
            _ => false
        };
    }

    /// <summary>
    /// Determines if the specified exception represents a transient error that can be retried.
    /// </summary>
    /// <param name="exception">The exception to evaluate.</param>
    /// <returns>True if the error is transient and the operation can be retried; otherwise, false.</returns>
    public bool IsTransientError(Exception exception)
    {
        // Check for SocketException directly
        if (exception is SocketException socketEx)
        {
            return IsTransientError(socketEx);
        }

        // Check for SocketException in inner exception
        if (exception.InnerException is SocketException innerSocketEx)
        {
            return IsTransientError(innerSocketEx);
        }

        // IOException can wrap socket errors
        if (exception is IOException ioEx && ioEx.InnerException is SocketException ioSocketEx)
        {
            return IsTransientError(ioSocketEx);
        }

        // TimeoutException is transient
        if (exception is TimeoutException)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Calculates the delay before the next retry attempt using exponential backoff with jitter.
    /// </summary>
    /// <param name="retryAttempt">The current retry attempt number (1-based).</param>
    /// <returns>The delay before the next retry.</returns>
    public TimeSpan CalculateDelay(int retryAttempt)
    {
        if (retryAttempt < 1)
        {
            return InitialDelay;
        }

        // Calculate exponential backoff
        var exponentialDelay = InitialDelay.TotalMilliseconds * Math.Pow(BackoffMultiplier, retryAttempt - 1);

        // Cap at max delay
        var cappedDelay = Math.Min(exponentialDelay, MaxDelay.TotalMilliseconds);

        // Add jitter
        double jitterAmount;
        lock (Jitter)
        {
            jitterAmount = Jitter.NextDouble() * JitterFactor * 2 - JitterFactor; // Range: [-JitterFactor, +JitterFactor]
        }
        var delayWithJitter = cappedDelay * (1 + jitterAmount);

        // Ensure delay is not negative
        return TimeSpan.FromMilliseconds(Math.Max(0, delayWithJitter));
    }

    /// <summary>
    /// Executes an async operation with retry logic.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The result of the operation.</returns>
    /// <exception cref="AggregateException">Thrown when all retry attempts fail.</exception>
    public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken)
    {
        if (MaxRetries == 0)
        {
            return await operation(cancellationToken).ConfigureAwait(false);
        }

        var exceptions = new List<Exception>();

        for (int attempt = 0; attempt <= MaxRetries; attempt++)
        {
            try
            {
                return await operation(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex) when (attempt < MaxRetries && !cancellationToken.IsCancellationRequested && IsTransientError(ex))
            {
                exceptions.Add(ex);
                var delay = CalculateDelay(attempt + 1);
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }

        throw new AggregateException($"Operation failed after {MaxRetries + 1} attempts.", exceptions);
    }
}

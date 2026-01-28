namespace Docker.DotNet.Handler.Abstractions;

public abstract class WriteClosableStream : Stream
{
    public abstract bool CanCloseWrite { get; }

    public abstract void CloseWrite();
}
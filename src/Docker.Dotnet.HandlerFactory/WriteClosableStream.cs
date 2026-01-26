namespace Docker.DotNet.HandlerFactory;

public abstract class WriteClosableStream : Stream
{
    public abstract bool CanCloseWrite { get; }

    public abstract void CloseWrite();
}
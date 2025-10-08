namespace Docker.DotNet;

public abstract class WriteClosableStream : Stream
{
    public abstract bool CanCloseWrite { get; }

    public abstract void CloseWrite();
}
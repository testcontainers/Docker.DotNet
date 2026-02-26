namespace Docker.DotNet.Handler.Abstractions;

public interface IStreamHijacker
{
    Task<WriteClosableStream> HijackStreamAsync(HttpContent content);
}
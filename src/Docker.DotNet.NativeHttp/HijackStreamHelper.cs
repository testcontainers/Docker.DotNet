namespace Docker.DotNet.NativeHttp;

public class HijackStreamHelper
{
    static public WriteClosableStream HijackStream(HttpContent content)
    {
        return new WriteClosableStreamWrapper(content.ReadAsStreamAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult());
    }
}

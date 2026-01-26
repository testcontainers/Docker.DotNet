namespace Docker.DotNet.Unix;

public class HijackStreamHelper
{
    static public WriteClosableStream HijackStream(HttpContent content)
    {
        if (content is not HttpConnectionResponseContent contentHijackAble)
        {
            throw new NotSupportedException("message handler does not support hijacked streams");
        }

        return contentHijackAble.HijackStream();
    }
}

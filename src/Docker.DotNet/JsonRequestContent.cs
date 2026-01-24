namespace Docker.DotNet;

internal class JsonRequestContent<T> : IRequestContent where T : class
{
    private readonly T _value;
    private readonly JsonSerializer _serializer;

    public JsonRequestContent(T val, JsonSerializer serializer)
    {
        if (val == null)
        {
            throw new ArgumentNullException(nameof(val));
        }

        if (serializer == null)
        {
            throw new ArgumentNullException(nameof(serializer));
        }

        _value = val;
        _serializer = serializer;
    }

    public HttpContent GetContent()
    {
        return _serializer.GetHttpContent(_value);
    }
}
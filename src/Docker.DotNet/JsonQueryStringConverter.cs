namespace Docker.DotNet.Models;

internal class JsonQueryStringConverter : IQueryStringConverter
{
    public bool CanConvert(Type t)
    {
        return true;
    }

    public string[] Convert(object o)
    {
        Debug.Assert(o != null);

        return new[] { JsonSerializer.Instance.Serialize(o) };
    }
}
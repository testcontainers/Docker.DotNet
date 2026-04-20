namespace Docker.DotNet;

internal sealed class QueryStringMapParameterAttribute<T>(string name, bool required) : QueryStringParameterAttribute(name, required)
{
    public override string[] Convert(object value)
    {
        Debug.Assert(value != null);
        Debug.Assert(value is T);

        return [JsonSerializer.Instance.Serialize((T)value!)];
    }
}

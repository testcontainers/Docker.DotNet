namespace Docker.DotNet;

internal sealed class QueryStringMapParameterAttribute<T>(string name, bool required) : QueryStringParameterAttribute(name, required)
{
    public override IEnumerable<string> Convert(object value)
    {
        Debug.Assert(value != null);
        Debug.Assert(value is T);

        if (value is not T typedValue)
        {
            throw new ArgumentException($"Expected value of type '{typeof(T)}'.", nameof(value));
        }

        return [JsonSerializer.Instance.Serialize(typedValue)];
    }
}
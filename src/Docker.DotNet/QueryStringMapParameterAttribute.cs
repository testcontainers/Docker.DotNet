namespace Docker.DotNet;

internal sealed class QueryStringMapParameterAttribute(Type type, string name, bool required) : QueryStringParameterAttribute(name, required)
{
    public override IEnumerable<string> Convert(object value)
    {
        Debug.Assert(value != null);
        Debug.Assert(type.IsInstanceOfType(value));

        if (!type.IsInstanceOfType(value))
        {
            throw new ArgumentException($"Expected value of type '{type}'.", nameof(value));
        }

        return [JsonSerializer.Instance.Serialize(value, type)];
    }
}
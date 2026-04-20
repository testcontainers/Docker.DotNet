namespace Docker.DotNet;

internal sealed class QueryStringListParameterAttribute(string name, bool required) : QueryStringParameterAttribute(name, required)
{
    public override string[] Convert(object value)
    {
        Debug.Assert(value != null);
        Debug.Assert(value is IList<string>);

        if (value is not IList<string> typedValue)
        {
            throw new ArgumentException($"Expected value of type '{typeof(IList<string>)}'.", nameof(value));
        }

        return typedValue.ToArray();
    }
}
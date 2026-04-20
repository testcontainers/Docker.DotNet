namespace Docker.DotNet;

internal sealed class QueryStringListParameterAttribute<T>(string name, bool required) : QueryStringParameterAttribute(name, required) where T : IList<string>
{
    public override string[] Convert(object value)
    {
        Debug.Assert(value != null);
        Debug.Assert(value is IList<string>);

        var enumerable = (IList<string>)value!;

        var items = new List<string>();

        foreach (var e in enumerable)
        {
            items.Add(e.ToString()!);
        }

        return items.ToArray();
    }
}

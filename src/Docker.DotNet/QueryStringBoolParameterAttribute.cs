namespace Docker.DotNet;

internal sealed class QueryStringBoolParameterAttribute(string name, bool required) : QueryStringParameterAttribute(name, required)
{
    public override string[] Convert(object value)
    {
        Debug.Assert(value != null);

        return [System.Convert.ToInt32(System.Convert.ToBoolean(value)).ToString(CultureInfo.InvariantCulture)];
    }
}

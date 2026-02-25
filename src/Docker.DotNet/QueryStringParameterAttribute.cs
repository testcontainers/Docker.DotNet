namespace Docker.DotNet;

[AttributeUsage(AttributeTargets.Property)]
internal sealed class QueryStringParameterAttribute : Attribute
{
    public string Name { get; private set; }

    public bool IsRequired { get; private set; }

    public Type? ConverterType { get; private set; }

    public QueryStringParameterAttribute(string name, bool required, Type? converterType = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (converterType != null && !converterType.GetInterfaces().Contains(typeof (IQueryStringConverter)))
        {
            throw new ArgumentException($"Provided query string converter type is not '{typeof(IQueryStringConverter).FullName}'.", nameof(converterType));
        }

        Name = name;
        IsRequired = required;
        ConverterType = converterType;
    }
}
namespace Docker.DotNet;

[AttributeUsage(AttributeTargets.Property)]
internal class QueryStringParameterAttribute : Attribute
{
    public string Name { get; private set; }

    public bool IsRequired { get; private set; }

    public virtual IQueryStringConverter? GetConverter() => null;

    public QueryStringParameterAttribute(string name, bool required)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;
        IsRequired = required;
    }
}

internal sealed class QueryStringParameterAttribute<TConverter>(string name, bool required) : QueryStringParameterAttribute(name, required) where TConverter : IQueryStringConverter, new()
{
    private static readonly ConcurrentDictionary<Type, IQueryStringConverter> ConverterInstanceRegistry = new ConcurrentDictionary<Type, IQueryStringConverter>();

    public override IQueryStringConverter GetConverter() => ConverterInstanceRegistry.GetOrAdd(typeof(TConverter), static _ => new TConverter());
}
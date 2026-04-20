namespace Docker.DotNet;

[AttributeUsage(AttributeTargets.Property)]
internal class QueryStringParameterAttribute : Attribute
{
    public string Name { get; private set; }

    public bool IsRequired { get; private set; }

    public virtual string[] Convert(object value) => [value.ToString()!];

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

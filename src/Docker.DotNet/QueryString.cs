namespace Docker.DotNet;

internal class QueryString<
#if NET
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
T> : IQueryString where T : class
{
    private T Object { get; }

    private Dictionary<PropertyInfo, QueryStringParameterAttribute> AttributedPublicProperties { get; }

    public QueryString(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Object = value;
        AttributedPublicProperties = FindAttributedPublicProperties<T, QueryStringParameterAttribute>();
    }

    /// <summary>
    /// Returns formatted query string.
    /// </summary>
    /// <returns></returns>
    public string GetQueryString()
    {
        var sb = new StringBuilder();
        foreach (var pair in AttributedPublicProperties)
        {
            var property = pair.Key;
            var attribute = pair.Value;
            var value = property.GetValue(Object, null);

            // 'Required' check
            if (attribute.IsRequired && value == null)
            {
                string propertyFullName = $"{property.DeclaringType?.FullName}.{property.Name}";
                throw new ArgumentException("Got null/unset value for a required query parameter.", propertyFullName);
            }

            // Serialize
            if (attribute.IsRequired || !IsDefaultOfType(value))
            {
                var keyStr = attribute.Name;

                foreach (var valueStr in attribute.Convert(value!))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append('&');
                    }
                    sb.Append(Uri.EscapeDataString(keyStr));
                    sb.Append('=');
                    sb.Append(Uri.EscapeDataString(valueStr));
                }
            }
        }

        return sb.ToString();
    }

    private static Dictionary<PropertyInfo, TAttribType> FindAttributedPublicProperties<
#if NET
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
#endif
    TValue, TAttribType>() where TAttribType : Attribute
    {
        Dictionary<PropertyInfo, TAttribType>? attributedPublicProperties = null;

        var t = typeof(TValue);

        foreach (var prop in t.GetProperties())
        {
            if (prop.GetGetMethod(false)?.IsPublic == true)
            {
                var attribute = prop.GetCustomAttribute<TAttribType>();
                if (attribute != null)
                {
                    attributedPublicProperties ??= new Dictionary<PropertyInfo, TAttribType>();
                    attributedPublicProperties.Add(prop, attribute);
                }
            }
        }

        if (attributedPublicProperties == null)
        {
            throw new InvalidOperationException($"No public properties attributed with [{typeof(TAttribType).FullName}] found on type {t.FullName}.");
        }

        return attributedPublicProperties;
    }

#if NET
    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Activator.CreateInstance is only used for value types here, safe for runtime usage.")]
#endif
    private static bool IsDefaultOfType(object? o)
    {
        if (o is ValueType)
        {
            return o.Equals(Activator.CreateInstance(o.GetType()));
        }

        return o == null;
    }
}

/// <summary>
/// Generates query string formatted as:
/// [url]?key=value1&amp;key=value2&amp;key=value3...
/// </summary>
internal class EnumerableQueryString : IQueryString
{
    private readonly string _key;
    private readonly string[] _data;

    public EnumerableQueryString(string key, string[] data)
    {
        _key = key;
        _data = data;
    }

    /// <summary>
    /// Returns formatted query string.
    /// </summary>
    /// <returns></returns>
    public string GetQueryString()
    {
        return string.Join("&", _data.Select(v => $"{Uri.EscapeDataString(_key)}={Uri.EscapeDataString(v)}"));
    }
}
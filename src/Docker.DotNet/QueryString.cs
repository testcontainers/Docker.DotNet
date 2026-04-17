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

    public IDictionary<string, string[]> GetKeyValuePairs()
    {
        var queryParameters = new Dictionary<string, string[]>();
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
                string[] valueStr;
                if (attribute.GetConverter() is { } converter)
                {
                    valueStr = ConvertValue(converter, value!);

                    if (valueStr == null)
                    {
                        throw new InvalidOperationException($"Got null from value converter '{converter.GetType().FullName}'");
                    }
                }
                else
                {
                    valueStr = [value!.ToString()!];
                }

                queryParameters[keyStr] = valueStr;
            }
        }

        return queryParameters;
    }

    /// <summary>
    /// Returns formatted query string.
    /// </summary>
    /// <returns></returns>
    public string GetQueryString()
    {
        return string.Join("&",
            GetKeyValuePairs().Select(
                pair => string.Join("&",
                    pair.Value.Select(
                        v => $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(v)}"))));
    }

    private static string[] ConvertValue(IQueryStringConverter converter, object value)
    {
        if (!converter.CanConvert(value.GetType()))
        {
            throw new InvalidOperationException(
                $"Cannot convert type {value.GetType().FullName} using {converter.GetType().FullName}.");
        }
        return converter.Convert(value);
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
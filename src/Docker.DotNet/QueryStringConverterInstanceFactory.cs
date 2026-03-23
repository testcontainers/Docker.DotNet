namespace Docker.DotNet;

internal class QueryStringConverterInstanceFactory : IQueryStringConverterInstanceFactory
{
    private static readonly ConcurrentDictionary<Type, IQueryStringConverter> ConverterInstanceRegistry = new ConcurrentDictionary<Type, IQueryStringConverter>();

    public IQueryStringConverter GetConverterInstance(
#if NET
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
        Type t)
    {
#pragma warning disable IL2111 // Method with parameters or return value with `DynamicallyAccessedMembersAttribute` is accessed via reflection. Trimmer can't guarantee availability of the requirements of the method.
        return ConverterInstanceRegistry.GetOrAdd(t, InitializeConverter);
#pragma warning restore IL2111 // Method with parameters or return value with `DynamicallyAccessedMembersAttribute` is accessed via reflection. Trimmer can't guarantee availability of the requirements of the method.
    }

    private static IQueryStringConverter InitializeConverter(
#if NET
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
        Type t)
    {
        if (Activator.CreateInstance(t) is IQueryStringConverter instance)
        {
            return instance;
        }

        throw new InvalidOperationException($"Could not get instance of {t.FullName}");
    }
}
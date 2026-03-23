namespace Docker.DotNet;

internal interface IQueryStringConverterInstanceFactory
{
    IQueryStringConverter GetConverterInstance(
#if NET
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
        Type t);
}
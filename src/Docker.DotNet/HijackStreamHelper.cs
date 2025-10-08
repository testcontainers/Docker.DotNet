namespace Docker.DotNet;

public class HijackStreamHelper
{
    public static MethodInfo GetHijackMethodFromType(Type type) => 
        type.GetMethod("HijackStream", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
}
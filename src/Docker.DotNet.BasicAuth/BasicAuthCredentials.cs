namespace Docker.DotNet.BasicAuth;

public class BasicAuthCredentials : IAuthProvider
{
    private readonly string _username;

    private readonly string _password;

    private BasicAuthCredentials(string username, string password, bool tlsEnabled = false)
    {
        _username = username;
        _password = password;
        TlsEnabled = tlsEnabled;
    }

    public bool TlsEnabled { get; }

    public HttpMessageHandler ConfigureHandler(HttpMessageHandler handler)
        => new BasicAuthHandler(_username, _password, handler);
}
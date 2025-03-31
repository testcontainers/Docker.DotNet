namespace Docker.DotNet.BasicAuth;

public class BasicAuthCredentials : Credentials
{
    private readonly bool _isTls;

    private readonly MaybeSecureString _username;

    private readonly MaybeSecureString _password;

    private bool _disposed;

    public BasicAuthCredentials(SecureString username, SecureString password, bool isTls = false)
        : this(new MaybeSecureString(username), new MaybeSecureString(password), isTls)
    {
    }

    public BasicAuthCredentials(string username, string password, bool isTls = false)
        : this(new MaybeSecureString(username), new MaybeSecureString(password), isTls)
    {
    }

    private BasicAuthCredentials(MaybeSecureString username, MaybeSecureString password, bool isTls)
    {
        _isTls = isTls;
        _username = username;
        _password = password;
    }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public override bool IsTlsCredentials()
    {
        return _isTls;
    }

    public override HttpMessageHandler GetHandler(HttpMessageHandler handler)
    {
        return new BasicAuthHandler(_username, _password, handler);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _username.Dispose();
            _password.Dispose();
        }

        _disposed = true;
    }
}
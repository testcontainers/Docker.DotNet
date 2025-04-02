namespace Docker.DotNet.BasicAuth;

internal class BasicAuthHandler : DelegatingHandler
{
    private readonly MaybeSecureString _username;

    private readonly MaybeSecureString _password;

    public BasicAuthHandler(MaybeSecureString username, MaybeSecureString password, HttpMessageHandler httpMessageHandler)
        : base(httpMessageHandler)
    {
        _username = username.Copy();
        _password = password.Copy();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _username.Dispose();
            _password.Dispose();
        }

        base.Dispose(disposing);
    }
}
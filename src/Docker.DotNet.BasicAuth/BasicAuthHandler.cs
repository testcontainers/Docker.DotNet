namespace Docker.DotNet.BasicAuth;

internal class BasicAuthHandler : DelegatingHandler
{
    private readonly MaybeSecureString _username;

    private readonly MaybeSecureString _password;

    private readonly Lazy<AuthenticationHeaderValue> _authHeader;

    public BasicAuthHandler(MaybeSecureString username, MaybeSecureString password, HttpMessageHandler httpMessageHandler)
        : base(httpMessageHandler)
    {
        _username = username.Copy();
        _password = password.Copy();

        _authHeader = new Lazy<AuthenticationHeaderValue>(() =>
        {
            var credentials = $"{_username}:{_password}";
            var bytes = Encoding.ASCII.GetBytes(credentials);
            var base64 = Convert.ToBase64String(bytes);
            return new AuthenticationHeaderValue("Basic", base64);
        });
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = _authHeader.Value;
        return base.SendAsync(request, cancellationToken);
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
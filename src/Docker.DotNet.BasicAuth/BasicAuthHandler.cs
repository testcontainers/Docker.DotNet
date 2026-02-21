namespace Docker.DotNet.BasicAuth;

internal sealed class BasicAuthHandler : DelegatingHandler
{
    private readonly Lazy<AuthenticationHeaderValue> _authHeader;

    public BasicAuthHandler(string username, string password, HttpMessageHandler httpMessageHandler)
        : base(httpMessageHandler)
    {
        _authHeader = new Lazy<AuthenticationHeaderValue>(() =>
        {
            var credentials = $"{username}:{password}";
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
}
namespace Docker.DotNet.X509;

public class CertificateCredentials : Credentials, IAuthProvider
{
    private readonly X509Certificate2 _certificate;

    public CertificateCredentials(X509Certificate2 certificate)
    {
        _certificate = certificate;
    }

    public bool TlsEnabled => true;

    public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public override bool IsTlsCredentials()
    {
        return true;
    }

    public override HttpMessageHandler GetHandler(HttpMessageHandler handler)
    {
#if NET6_0_OR_GREATER
        if (handler is SocketsHttpHandler socketsHandler)
        {
            socketsHandler.SslOptions.ClientCertificates = new X509Certificate2Collection();
            socketsHandler.SslOptions.ClientCertificates.Add(_certificate);
            socketsHandler.SslOptions.RemoteCertificateValidationCallback = ServerCertificateValidationCallback;
            return socketsHandler;
        }
#else
        if (handler is HttpClientHandler httpHandler)
        {
            httpHandler.ClientCertificates.Add(_certificate);
            httpHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => ServerCertificateValidationCallback(message, certificate, chain, sslPolicyErrors);
            return httpHandler;
        }
#endif
        if (handler is not ManagedHandler managedHandler)
        {
            return handler;
        }

        if (!managedHandler.ClientCertificates.Contains(_certificate))
        {
            managedHandler.ClientCertificates.Add(_certificate);
        }

        managedHandler.ServerCertificateValidationCallback = ServerCertificateValidationCallback;

        return handler;
    }

    public HttpMessageHandler ConfigureHandler(HttpMessageHandler handler)
    {
        return GetHandler(handler);
    }

    protected virtual void Dispose(bool _)
    {
    }
}
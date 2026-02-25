namespace Docker.DotNet.X509;

public class CertificateCredentials : IAuthProvider
{
    private readonly X509Certificate2 _certificate;

    public CertificateCredentials(X509Certificate2 certificate)
    {
        _certificate = certificate;
    }

    public bool TlsEnabled => true;

    public RemoteCertificateValidationCallback ServerCertificateValidationCallback { get; set; }

    public HttpMessageHandler ConfigureHandler(HttpMessageHandler handler)
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
}
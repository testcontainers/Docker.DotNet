namespace Docker.DotNet.X509;

public class CertificateCredentials : IAuthProvider
{
    private readonly X509Certificate2? _certificate;

    public CertificateCredentials(X509Certificate2? certificate)
    {
        _certificate = certificate;
    }

    public bool TlsEnabled => true;

    public RemoteCertificateValidationCallback? ServerCertificateValidationCallback { get; set; }

    public HttpMessageHandler ConfigureHandler(HttpMessageHandler handler)
    {
#if NET
        if (handler is SocketsHttpHandler socketsHandler)
        {
            if (_certificate != null)
            {
                socketsHandler.SslOptions.ClientCertificates = new X509Certificate2Collection();
                socketsHandler.SslOptions.ClientCertificates.Add(_certificate);
            }

            socketsHandler.SslOptions.RemoteCertificateValidationCallback = ServerCertificateValidationCallback;
            return socketsHandler;
        }
#else
        if (handler is HttpClientHandler httpHandler)
        {
            if  (_certificate != null && !httpHandler.ClientCertificates.Contains(_certificate))
            {
                httpHandler.ClientCertificates.Add(_certificate);
            }

            if (ServerCertificateValidationCallback is { } serverCertificateValidationCallback)
            {
                httpHandler.ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => serverCertificateValidationCallback(message, certificate, chain, sslPolicyErrors);
            }

            return httpHandler;
        }
#endif
        if (handler is ManagedHandler managedHandler)
        {
            if  (_certificate != null && !managedHandler.ClientCertificates.Contains(_certificate))
            {
                managedHandler.ClientCertificates.Add(_certificate);
            }

            managedHandler.ServerCertificateValidationCallback = ServerCertificateValidationCallback;
            return managedHandler;
        }

        return handler;
    }
}